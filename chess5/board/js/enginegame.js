function engineGame(options) {
    options = options || {};
    var game = new Chess();
    var board;
    var engine = new Worker(options.stockfishjs || 'stockfish.js');
    var engineStatus = {};
    var displayScore = false;
    var time = { wtime: 300000, btime: 300000, winc: 2000, binc: 2000 };
    var playerColor = 'white';
    var clockTimeoutID = null;
    var isEngineRunning = false;
    var boardEl = $('#board');
    var squareToHighlight;
    var colorToHighlight;
    var game_history_fen = [];
    var game_history_pgn;
    var removeGreySquares = function() {
        $('#board .cb_square').removeClass(['cb_overlay_l', 'cb_overlay_d']);
    };
    var greySquare = function(square) {
        var squareEl = $('#board .cb_square-' + square);
        var cls = 'cb_overlay_l';
        if (squareEl.hasClass('cb_black') === true) 
            cls = 'cb_overlay_d';
        squareEl.addClass(cls);
    };
    var onDragStart = function(source, piece, position, orientation) {
        var re = playerColor == 'white' ? /^b/ : /^w/
        if (game.game_over() || piece.search(re) !== -1)
            return false;
    };
    var onMouseoverSquare = function(square, piece) {
        // get list of possible moves for this square
        var moves = game.moves({
            square: square,
            verbose: true
        });
        // exit if there are no moves available for this square
        if (moves.length === 0) return;
        // highlight the square they moused over
        greySquare(square);
        // highlight the possible squares for this piece
        for (var i = 0; i < moves.length; i++) {
            greySquare(moves[i].to);
        }
    };
    var onMouseoutSquare = function(square, piece) {
        removeGreySquares();
    };
    var onDrop = function(source, target, piece) {
        removeGreySquares();
        // see if the move is legal
        var move = game.move({
            from: source,
            to: target,
            promotion: 'q' // NOTE: always promote to a pawn for example simplicity
        });
        // illegal move
        if (move === null) return 'snapback';
        prepareMove();
    };
    var onSnapEnd = function() {
        board.position(game.fen());
    };
    var squareHighlight = function() {
        var history = game.history({verbose: true});
        var hlen = history.length;
        if(hlen === 0) return;
        boardEl.find('.cb_square').removeClass(['cb_highlight-black', 'cb_highlight-white']);
        if (history[hlen-1].color === 'w') {
            boardEl.find('.cb_square-' + history[hlen-1].from).addClass('cb_highlight-white');
            squareToHighlight = history[hlen-1].to;
            colorToHighlight = 'white';
        }
        else {
            boardEl.find('.cb_square-' + history[hlen-1].from).addClass('cb_highlight-black');
            squareToHighlight = history[hlen-1].to;
            colorToHighlight = 'black';
        }
    };
    var onMoveEnd = function() {
        boardEl.find('.cb_square-' + squareToHighlight).addClass('cb_highlight-' + colorToHighlight);
    };
    function uciCmd(cmd) {
        engine.postMessage(cmd);
    }
    uciCmd('uci');
    function displayStatus() {
        var status = 'Engine: ';
        if(!engineStatus.engineLoaded)
            status += 'loading...';
        else if(!engineStatus.engineReady)
            status += 'loaded...';
        else
            status += 'ready.';
        status += ' Book: ' + engineStatus.book;
        if(engineStatus.search) {
            status += '<br>' + engineStatus.search;
            if(engineStatus.score && displayScore)
                status += ' Score: ' + engineStatus.score;
        }
        //$('#engineStatus').html(status);
    }
    function displayClock(color, t) {
        var isRunning = false;
        if(time.startTime > 0 && color === time.clockColor) {
            t = Math.max(0, t + time.startTime - Date.now());
            isRunning = true;
        }
        var id = color === playerColor ? '#time2' : '#time1';
        var sec = Math.ceil(t / 1000);
        var min = Math.floor(sec / 60);
        sec -= min * 60;
        var hours = Math.floor(min / 60);
        min -= hours * 60;
        var display = hours + ':' + ('0' + min).slice(-2) + ':' + ('0' + sec).slice(-2);
        if(isRunning)
            display += sec & 1 ? ' <--' : ' <-';
        $(id).text(display);
    }
    function updateClock() {
        displayClock('white', time.wtime);
        displayClock('black', time.btime);
    }
    function clockTick() {
        updateClock();
        var t = (time.clockColor == 'white' ? time.wtime : time.btime) + time.startTime - Date.now();
        var timeToNextSecond = (t % 1000) + 1;
        clockTimeoutID = setTimeout(clockTick, timeToNextSecond);
    }
    function stopClock() {
        if(clockTimeoutID !== null) {
            clearTimeout(clockTimeoutID);
            clockTimeoutID = null;
        }
        if(time.startTime > 0) {
            var elapsed = Date.now() - time.startTime;
            time.startTime = null;
            if(time.clockColor == 'white')
                time.wtime = Math.max(0, time.wtime - elapsed);
            else
                time.btime = Math.max(0, time.btime - elapsed);
        }
    }
    function startClock() {
        if(game.turn() == 'w') {
            time.wtime += time.winc;
            time.clockColor = 'white';
        } else {
            time.btime += time.binc;
            time.clockColor = 'black';
        }
        time.startTime = Date.now();
        clockTick();
    }
    function displayHistoryPgn(){
        var history = game.history({verbose: true});
        var html = '';
        var line = 0;
        var pgnEl = $('#pgn');
        if(history.length > 0) {
            history.forEach(function(item, i){
                if(i % 2 === 0) {
                    line ++;
                    html += '<div class="line"><span>#'+line+' </span><span class="pgn--'+item.color+'"> '+item.to+'</span>';
                } else
                    html += '<span class="pgn--'+item.color+'"> '+item.to+'</span></div>';
            });
            pgnEl.append(html);
            pgnEl.scrollTop(pgnEl.get(0).scrollHeight);
            
        } else{
            //$('#pgn').html('');
        }
    }
    function prepareMove() {
        if(typeof getRemovedPiece === 'function')
            getRemovedPiece(game.fen());
        stopClock();
        console.log(game.pgn());
        //console.log(game.history({verbose: true}));
        //$('#pgn').html(game.pgn({ max_width: 1, newline_char: '<br />' }));
        board.position(game.fen());
        //displayHistoryPgn();
        updateClock();
        var turn = game.turn() === 'w' ? 'white' : 'black';
        if(!game.game_over()) {
            squareHighlight();
            game_history_fen.push(game.fen());
            //game_history_pgn.push(game.history({verbose: true}));
            displayHistoryPgn();
            if(turn !== playerColor) {
                var moves = '';
                var history = game.history({verbose: true});
                for(var i = 0; i < history.length; ++i) {
                    var move = history[i];
                    moves += ' ' + move.from + move.to + (move.promotion ? move.promotion : '');
                }
                uciCmd('position startpos moves' + moves);
                if(time.depth)
                    uciCmd('go depth ' + time.depth);
                else if(time.nodes)
                    uciCmd('go nodes ' + time.nodes);
                else
                    uciCmd('go wtime ' + time.wtime + ' winc ' + time.winc + ' btime ' + time.btime + ' binc ' + time.binc);
                isEngineRunning = true;
            }
            if(game.history().length >= 1 && !time.depth && !time.nodes)
                startClock();
        }
    }
    engine.onmessage = function(event) {
        var line = event.data;
        if(line === 'uciok')
            engineStatus.engineLoaded = true;
        else if(line === 'readyok')
            engineStatus.engineReady = true;
        else {
            var match = line.match(/^bestmove ([a-h][1-8])([a-h][1-8])([qrbk])?/);
            if(match) {
                isEngineRunning = false;
                game.move({from: match[1], to: match[2], promotion: match[3]});
                prepareMove();
            } else if(match = line.match(/^info .*\bdepth (\d+) .*\bnps (\d+)/))
                engineStatus.search = 'Depth: ' + match[1] + ' Nps: ' + match[2];
            if(match = line.match(/^info .*\bscore (\w+) (-?\d+)/)) {
                var score = parseInt(match[2]) * (game.turn() === 'w' ? 1 : -1);
                if(match[1] === 'cp')
                    engineStatus.score = (score / 100.0).toFixed(2);
                else if(match[1] === 'mate')
                    engineStatus.score = '#' + score;
                if(match = line.match(/\b(upper|lower)bound\b/))
                    engineStatus.score = ((match[1] === 'upper') === (game.turn() === 'w') ? '<= ' : '>= ') + engineStatus.score;
            }
        }
        displayStatus();
    };
    if(options.book) {
        var bookRequest = new XMLHttpRequest();
        bookRequest.open('GET', options.book, true);
        bookRequest.responseType = "arraybuffer";
        bookRequest.onload = function(event) {
            if(bookRequest.status == 200) {
                engine.postMessage({book: bookRequest.response});
                engineStatus.book = 'ready.';
                displayStatus();
            } else {
                engineStatus.book = 'failed!';
                displayStatus();
            }
        };
        bookRequest.send(null);
    } else
        engineStatus.book = 'none';
    var cfg = {
        draggable: true,
        position: 'start',
        onDragStart: onDragStart,
        onDrop: onDrop,
        onSnapEnd: onSnapEnd,
        onMoveEnd: onMoveEnd,
        onMouseoutSquare: onMouseoutSquare,
        onMouseoverSquare: onMouseoverSquare,
        pieceTheme: './img/tour2/{piece}.png'
    };
    board = new ChessBoard('board', cfg);
    return {
        reset: function() {
            game.reset();
            uciCmd('setoption name Contempt Factor value 0');
            uciCmd('setoption name Skill Level value 20');
            uciCmd('setoption name Aggressiveness value 100');
        },
        loadPgn: function(pgn) { game.load_pgn(pgn);},
        setPlayerColor: function(color) {
            playerColor = color;
            board.orientation(playerColor);
        },
        setSkillLevel: function(skill) {
            uciCmd('setoption name Skill Level value ' + skill);
        },
        setTime: function(baseTime, inc) {
            time = { wtime: baseTime * 1000, btime: baseTime * 1000, winc: inc * 1000, binc: inc * 1000 };
        },
        setDepth: function(depth) {
            time = { depth: depth };
        },
        setNodes: function(nodes) {
            time = { nodes: nodes };
        },
        setContempt: function(contempt) {
            uciCmd('setoption name Contempt Factor value ' + contempt);
        },
        setAggressiveness: function(value) {
            uciCmd('setoption name Aggressiveness value ' + value);
        },
        setDisplayScore: function(flag) {
            displayScore = flag;
            displayStatus();
        },
        start: function() {
            uciCmd('ucinewgame');
            uciCmd('isready');
            engineStatus.engineReady = false;
            engineStatus.search = null;
            displayStatus();
            prepareMove();
        },
        startClock: function() {
            startClock();
        },
        undo: function() {
            if(isEngineRunning)
                return false;
            game.undo();
            game.undo();
            engineStatus.search = null;
            displayStatus();
            prepareMove();
            return true;
        },
        getHistoryFen: function() {
            return game_history_fen;
        },
        getBoard: function() {
            return board;
        },
        boardResize: function() {
            board.resize();
        },
        setPieceTheme: function(piece_theme) {
            cfg.pieceTheme = piece_theme;
        }
    };
}