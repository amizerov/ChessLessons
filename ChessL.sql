USE [ChessL]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[White_ID] [nvarchar](128) NULL,
	[Black_ID] [nvarchar](128) NULL,
	[BaseTime] [int] NULL,
	[Increment] [int] NULL,
	[WOnLineTime] [datetime] NULL,
	[BOnLineTime] [datetime] NULL,
	[PGN] [nvarchar](max) NULL,
	[Status] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GMove]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GMove](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Game_ID] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Color] [nvarchar](5) NOT NULL,
	[Move] [nvarchar](5) NOT NULL,
	[FEN] [nvarchar](150) NOT NULL,
	[PGN] [nvarchar](max) NOT NULL,
	[Time] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_GMove] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Level_ID] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Position] [varchar](500) NULL,
	[Orientation] [nchar](5) NULL,
	[OrderNumb] [int] NULL,
	[RatingFor] [int] NULL,
	[dtc] [datetime] NULL,
 CONSTRAINT [PK_Lesson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LLevel]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LLevel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_LLevel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Move]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Move](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Position] [varchar](150) NULL,
	[OrderNumb] [int] NULL,
	[Correctness] [int] NULL,
	[Step_ID] [int] NOT NULL,
	[dtc] [datetime] NULL,
 CONSTRAINT [PK_Move] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PMove]    Script Date: 01.03.2019 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PMove](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Step_ID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Position] [varchar](150) NOT NULL,
	[OrderNumb] [int] NOT NULL,
	[Correctness] [int] NOT NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_PMove] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PStep]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PStep](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Puzzle_ID] [int] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Position] [nvarchar](150) NOT NULL,
	[Orientation] [nvarchar](50) NOT NULL,
	[OrderNumb] [int] NOT NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_PStep] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PTema]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PTema](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](max) NULL,
	[OrderNumb] [int] NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_PTema] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Puzzle]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puzzle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tema_ID] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Position] [nvarchar](150) NOT NULL,
	[Orientation] [nvarchar](50) NOT NULL,
	[Rating] [int] NOT NULL,
	[OrderNumb] [int] NULL,
	[dtc] [datetime] NOT NULL,
 CONSTRAINT [PK_Zadacha] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Step]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Step](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Position] [nvarchar](150) NULL,
	[Orientation] [nchar](5) NULL,
	[Description] [nvarchar](max) NULL,
	[OrderNumb] [int] NULL,
	[Color] [nchar](1) NULL,
	[IsLast] [bit] NULL,
	[Topic_ID] [int] NOT NULL,
	[dtc] [datetime] NULL,
 CONSTRAINT [PK_Step] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Topic]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topic](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Position] [varchar](150) NULL,
	[Orientation] [nchar](5) NULL,
	[OrderNumb] [int] NULL,
	[InArticle] [bit] NOT NULL,
	[Lesson_ID] [int] NOT NULL,
	[dtc] [datetime] NULL,
 CONSTRAINT [PK_Topic] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLesson]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLesson](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](128) NOT NULL,
	[Lesson_ID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[dtc] [datetime] NOT NULL,
	[dtu] [datetime] NOT NULL,
 CONSTRAINT [PK_UserLesson] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPuzzle]    Script Date: 01.03.2019 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPuzzle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](128) NOT NULL,
	[Puzzle_ID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[IsComplete] [bit] NOT NULL,
	[dtc] [datetime] NOT NULL,
	[dtu] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPuzzle] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Game] ADD  CONSTRAINT [DF_Game_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Game] ADD  CONSTRAINT [DF_Game_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[GMove] ADD  CONSTRAINT [DF_GMove_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[Lesson] ADD  CONSTRAINT [DF_Lesson_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[LLevel] ADD  CONSTRAINT [DF_LLevel_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[Move] ADD  CONSTRAINT [DF_Move_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[PMove] ADD  CONSTRAINT [DF_PMove_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[PStep] ADD  CONSTRAINT [DF_PStep_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[PTema] ADD  CONSTRAINT [DF_PTema_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[Puzzle] ADD  CONSTRAINT [DF_Zadacha_Orientation]  DEFAULT ('white') FOR [Orientation]
GO
ALTER TABLE [dbo].[Puzzle] ADD  CONSTRAINT [DF_Zadacha_Rating]  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Puzzle] ADD  CONSTRAINT [DF_Zadacha_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[Step] ADD  CONSTRAINT [DF_Step_IsLast]  DEFAULT ((0)) FOR [IsLast]
GO
ALTER TABLE [dbo].[Step] ADD  CONSTRAINT [DF_Step_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[Topic] ADD  CONSTRAINT [DF_Topic_InArticle]  DEFAULT ((1)) FOR [InArticle]
GO
ALTER TABLE [dbo].[Topic] ADD  CONSTRAINT [DF_Topic_Lesson_ID]  DEFAULT ((1)) FOR [Lesson_ID]
GO
ALTER TABLE [dbo].[Topic] ADD  CONSTRAINT [DF_Topic_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[UserLesson] ADD  CONSTRAINT [DF_UserLesson_Count]  DEFAULT ((1)) FOR [Count]
GO
ALTER TABLE [dbo].[UserLesson] ADD  CONSTRAINT [DF_UserLesson_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
GO
ALTER TABLE [dbo].[UserLesson] ADD  CONSTRAINT [DF_UserLesson_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[UserLesson] ADD  CONSTRAINT [DF_UserLesson_dtu]  DEFAULT (getdate()) FOR [dtu]
GO
ALTER TABLE [dbo].[UserPuzzle] ADD  CONSTRAINT [DF_UserPuzzle_Count]  DEFAULT ((1)) FOR [Count]
GO
ALTER TABLE [dbo].[UserPuzzle] ADD  CONSTRAINT [DF_UserPuzzle_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
GO
ALTER TABLE [dbo].[UserPuzzle] ADD  CONSTRAINT [DF_UserPuzzle_dtc]  DEFAULT (getdate()) FOR [dtc]
GO
ALTER TABLE [dbo].[UserPuzzle] ADD  CONSTRAINT [DF_UserPuzzle_dtu]  DEFAULT (getdate()) FOR [dtu]
GO
ALTER TABLE [dbo].[GMove]  WITH CHECK ADD  CONSTRAINT [FK_GMove_Game] FOREIGN KEY([Game_ID])
REFERENCES [dbo].[Game] ([ID])
GO
ALTER TABLE [dbo].[GMove] CHECK CONSTRAINT [FK_GMove_Game]
GO
ALTER TABLE [dbo].[Lesson]  WITH CHECK ADD  CONSTRAINT [FK_Lesson_LLevel] FOREIGN KEY([Level_ID])
REFERENCES [dbo].[LLevel] ([ID])
GO
ALTER TABLE [dbo].[Lesson] CHECK CONSTRAINT [FK_Lesson_LLevel]
GO
ALTER TABLE [dbo].[Move]  WITH CHECK ADD  CONSTRAINT [FK_Move_Step] FOREIGN KEY([Step_ID])
REFERENCES [dbo].[Step] ([ID])
GO
ALTER TABLE [dbo].[Move] CHECK CONSTRAINT [FK_Move_Step]
GO
ALTER TABLE [dbo].[PMove]  WITH CHECK ADD  CONSTRAINT [FK_PMove_PStep] FOREIGN KEY([Step_ID])
REFERENCES [dbo].[PStep] ([ID])
GO
ALTER TABLE [dbo].[PMove] CHECK CONSTRAINT [FK_PMove_PStep]
GO
ALTER TABLE [dbo].[PStep]  WITH CHECK ADD  CONSTRAINT [FK_PStep_Puzzle] FOREIGN KEY([Puzzle_ID])
REFERENCES [dbo].[Puzzle] ([ID])
GO
ALTER TABLE [dbo].[PStep] CHECK CONSTRAINT [FK_PStep_Puzzle]
GO
ALTER TABLE [dbo].[Puzzle]  WITH CHECK ADD  CONSTRAINT [FK_Puzzle_PTema] FOREIGN KEY([Tema_ID])
REFERENCES [dbo].[PTema] ([ID])
GO
ALTER TABLE [dbo].[Puzzle] CHECK CONSTRAINT [FK_Puzzle_PTema]
GO
ALTER TABLE [dbo].[Step]  WITH CHECK ADD  CONSTRAINT [FK_Step_Topic] FOREIGN KEY([Topic_ID])
REFERENCES [dbo].[Topic] ([ID])
GO
ALTER TABLE [dbo].[Step] CHECK CONSTRAINT [FK_Step_Topic]
GO
ALTER TABLE [dbo].[Topic]  WITH CHECK ADD  CONSTRAINT [FK_Topic_Lesson] FOREIGN KEY([Lesson_ID])
REFERENCES [dbo].[Lesson] ([ID])
GO
ALTER TABLE [dbo].[Topic] CHECK CONSTRAINT [FK_Topic_Lesson]
GO
ALTER TABLE [dbo].[UserLesson]  WITH CHECK ADD  CONSTRAINT [FK_UserLesson_AspNetUsers] FOREIGN KEY([User_ID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserLesson] CHECK CONSTRAINT [FK_UserLesson_AspNetUsers]
GO
ALTER TABLE [dbo].[UserLesson]  WITH CHECK ADD  CONSTRAINT [FK_UserLesson_Lesson] FOREIGN KEY([Lesson_ID])
REFERENCES [dbo].[Lesson] ([ID])
GO
ALTER TABLE [dbo].[UserLesson] CHECK CONSTRAINT [FK_UserLesson_Lesson]
GO
/****** Object:  StoredProcedure [dbo].[Game_CreateGame]    Script Date: 01.03.2019 20:29:22 ******/
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
/****** Object:  StoredProcedure [dbo].[Game_GetMove]    Script Date: 01.03.2019 20:29:22 ******/
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
/****** Object:  StoredProcedure [dbo].[Game_GetSopernik]    Script Date: 01.03.2019 20:29:22 ******/
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
/****** Object:  StoredProcedure [dbo].[Game_SetMove]    Script Date: 01.03.2019 20:29:22 ******/
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
