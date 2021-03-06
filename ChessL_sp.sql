USE [ChessL]
GO
/****** Object:  StoredProcedure [dbo].[Game_CreateGame]    Script Date: 07.04.2019 18:08:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Game_CreateGame]
	@User_ID varchar(128),
	@BaseTime int,
	@Increment int
as
begin
	if len(@User_ID) = 0 return

	declare @White_ID varchar(128), @Black_ID varchar(128)
	declare @Game_ID int set @Game_ID = 0
	declare @Color char(5)

	-- 0 -- Что бы не плодить игры без ответа, берем ту, которую открыли ранее
	select @Game_ID = ID from Game g 
		where (select count(*) from GMove where Game_ID = g.ID) = 0
		  and IsNull(White_ID, '') = @User_ID and Black_ID is null
	if @Game_ID > 0 begin
		select @Game_ID Game_ID, 'w' Color
		return
	end 
	select @Game_ID = ID from Game g 
		where (select count(*) from GMove where Game_ID = g.ID) = 0
		  and IsNull(Black_ID, '') = @User_ID and White_ID is null
	if @Game_ID > 0 begin
		select @Game_ID Game_ID, 'b' Color
		return
	end 
	
	-- 1 -- Найти игру, у которой ещё нет ходов и один из цветов еще не занят, а один занят но не мной
	select @Game_ID = ID, @White_ID = White_ID, @Black_ID = Black_ID from Game g 
		where (select count(*) from GMove where Game_ID = g.ID) = 0
		  and (IsNull(White_ID, '')  <> @User_ID and IsNull(Black_ID, '') <> @User_ID)
		  and (White_ID is null or Black_ID is null)
	
	-- 2 -- Если такая игра нашлась, то подключаемся к ней тем цветом, который еще не занят
	if @Game_ID > 0 begin
		if @White_ID is null begin 
			update Game set White_ID = @User_ID where ID = @Game_ID
			set @Color = 'w'
		end else begin
			update Game set Black_ID = @User_ID where ID = @Game_ID
			set @Color = 'b'
		end
	end
	else begin

	-- 3 -- Если такой игры нет, то выбираем цвет случайно и создаем запрос на игру
		declare @s int set @s = datepart(ss, getdate()) % 2
		if @s = 1 begin
			insert Game(White_ID, BaseTime, Increment) values(@User_ID, @BaseTime, @Increment)
			set @Color = 'w'
		end else begin
			insert Game(Black_ID, BaseTime, Increment) values(@User_ID, @BaseTime, @Increment)
			set @Color = 'b'
		end
		set @Game_ID = @@IDENTITY 
	end

	select @Game_ID Game_ID, @Color Color
end
GO
/****** Object:  StoredProcedure [dbo].[Game_GetMove]    Script Date: 07.04.2019 18:08:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Game_GetMove] 
	@Game_ID int
as
	declare @UserWhite nvarchar(128), @UserBlack nvarchar(128)
	declare @Color char(1), @Move varchar(50), @FEN nvarchar(150), @Status nvarchar(50)
	
	select @UserWhite = w.UserName , @UserBlack = b.UserName  
	from Game g 
	left outer join AspNetUsers w on w.Id = isNull(g.White_ID, '')
	left outer join AspNetUsers b on b.Id = IsNull(g.Black_ID, '')
	where g.ID = @Game_ID

	select top 1 @Color = Color, @Move = Move, @FEN = FEN, @Status = [Status]
	from GMove 
	where Game_ID = @Game_ID order by dtc desc

	select @Color Color, @Move [Move], @FEN FEN, @Status [Status], @UserWhite UserWhite, @UserBlack UserBlack 
GO
/****** Object:  StoredProcedure [dbo].[Game_GetSopernik]    Script Date: 07.04.2019 18:08:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Game_GetSopernik]
	@Game_ID int,
	@MyColor char(1)
as

	if @MyColor = 'w'
		select u.UserName Sopernik 
		from Game g join AspNetUsers u on g.Black_ID = u.Id
		where g.ID = @Game_ID
	else 
	if @MyColor = 'b'
		select u.UserName Sopernik 
		from Game g join AspNetUsers u on g.White_ID = u.Id
		where g.ID = @Game_ID
GO
/****** Object:  StoredProcedure [dbo].[Game_SetMove]    Script Date: 07.04.2019 18:08:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[Game_SetMove]
	@Game_ID int, 
	@MoveNumb int, 
	@MoveColor nvarchar(5), 
	@Move nvarchar(5), 
	@FEN nvarchar(150), 
	@PGN nvarchar(max), 
	@Status nvarchar(50),
	@Time int 
as
begin

	if @Game_ID = 0 begin 
		insert Game(White_ID, Black_ID) values(1, 2)
		set @Game_ID = @@IDENTITY
	end

	INSERT GMove(Game_ID, Number, Color, Move, FEN, PGN, [Status], [Time])
			VALUES(@Game_ID, @MoveNumb, @MoveColor, @Move, @FEN, @PGN, @Status, @Time)
	
	if @Status = 'Mate' 
		update Game set PGN = @PGN, [Status] = @Status, IsActive = 0 where ID = @Game_ID

	select @Game_ID
end
GO
/****** Object:  StoredProcedure [dbo].[News_GetPuzzleOfDay]    Script Date: 07.04.2019 18:08:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[News_GetPuzzleOfDay] 
AS
BEGIN

	declare @Puzzle_ID int

	select @Puzzle_ID = ID 
	from Puzzle where PuzzleOfDay = 1

	if @Puzzle_ID is null
		select @Puzzle_ID = ID from 
			(select top 1 Puzzle_ID ID, count(*) c from UserPuzzle where dtc>getdate()-5 group by Puzzle_ID order by c desc) v

	select @Puzzle_ID 
		
END
GO
