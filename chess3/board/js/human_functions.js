
function request_offer_draw() {
    showed_reject_offer_draw = false;
    showed_success_offer_draw = false;
    $('#offer_draw__hide_request').hide();
    GameStatus = "RequestDraw." + MyColor;
    SetMove();
    $('#offer_draw__content').html('<div class="offer_draw__preloader_container"><div id="offer_draw__preloader" class="preloader_bounce">'
        + '<div class="p_b1"></div><div class="p_b2"></div><div class="p_b3"></div>'
        + '</div>'
        + '<h3>Ожидаем ответ соперника</h3></div>');
};
function success_offer_draw() {
    if(showed_success_offer_draw) {
        return;
    }
    showed_success_offer_draw = true;
    $('#offer_draw__hide_request').hide();
    modal_open('confirm_offer_draw');
    $('#offer_draw__content').html('<div class="modal__form__title"><h3>Соперник согласился</h3></div><br><button class="btn btn-primary" onclick="clear_result_offer_draw(true);">Ok</button>');
};
function reject_offer_draw() {
    if(showed_reject_offer_draw) {
        return;
    }
    showed_reject_offer_draw = true;
    $('#offer_draw__hide_request').hide();
    modal_open('confirm_offer_draw');
    $('#offer_draw__content').html('<div class="modal__form__title"><h3>Соперник отказался</h3></div><br><button class="btn btn-primary" onclick="clear_result_offer_draw(false);">Ok</button>');
};
function clear_result_offer_draw(stop_game) {
    modal_close('confirm_offer_draw');
    if(stop_game) {
        gameOverResult = {r1: 'draw', r2: '', r3: ''};
        SetGameResult(0);
    }
    setTimeout(function () {
        $('#offer_draw__content').html('');
        $('#offer_draw__hide_request').show();
    }, 500);
};
function send_success_draw() {
    modal_close('request_draw');
    GameStatus = "SuccessDraw." + MyColor;
    SetMove();
    gameOverResult = {r1: 'draw', r2: false, r3: false};
    SetGameResult(0);
};
function send_reject_draw() {
    modal_close('request_draw');
    GameStatus = "RejectDraw." + MyColor;
    SetMove();
};
function request_surrender() {
    modal_close('confirm_surrender');
    GameStatus = "Resign";
    SetMove();
    gameOverResult = {r1: 'lose', r2: false, r3: false};
    SetGameResult(0);
};
function show_modal_new_game(coin, coinD, ratg, ratgD) {
    if (coin === undefined)
        coin = false;
    if (coinD === undefined)
        coinD = false;
    if (ratg === undefined)
        ratg = false;
    if (ratgD === undefined)
        ratgD = false;

    var text = '', stat = '', func = '';

    if (gameOverResult.r3 === 'checkmate')
        text += 'Мат! ';
    if (gameOverResult.r3 === 'stalemate')
        text += 'Пат! ';

    if (gameOverResult.r1 === 'win') {
        text += 'Вы выиграли!';
        func = '';
        if (gameOverResult.r2 === 'surrender')
            text += ' Соперник сдался';
        else if (gameOverResult.r2 === 'close')
            text += ' Соперник покинул игру';
        else if (gameOverResult.r2 === 'time')
            text += ' У соперника закончилось время';
    } else if (gameOverResult.r1 === 'lose') {
        text += 'Вы проиграли!';
        func = '';
        if (gameOverResult.r2 === 'time')
            text += ' У вас закочилось время';
    } else if (gameOverResult.r1 === 'draw') {
        text = 'Ничья!';
        func = '';
    } else {
        return;
    }

    if (ratg && ratgD) {
        if (ratgD.toString().indexOf('-') === -1)
            ratgD = '+' + ratgD;
        stat += '<p>Ваш рейтинг: ' + ratg + ' ( ' + ratgD + ' )</p>';
    }

    if (coin && coinD) {
        if (coinD.toString().indexOf('-') === -1)
            coinD = '+' + coinD;
        stat += '<p>Chess Coins: ' + coin + ' ( ' + coinD + ' )</p>';
    }
    clear_result_modal_new_game();
    $('#modal_new_game_wrapper').find('.form-wrapper').prepend('<div id="modal_new_game__result">'
        + '<div class="modal__form__title">'
        + '<p>' + text + '</p>'
        + stat + '</div>'
        + '<button class="btn btn-primary w_auto" onclick="' + func + '">Реванш</button>'
        + '<a href="/Game" class="btn btn-default w_auto">Новая игра</a></div>');
    stop = true;
    modal_open('new_game');
};
function clear_result_modal_new_game() {
    if ($('#modal_new_game__result').length > 0)
        $('#modal_new_game__result').remove();
};
$(function () {
    mount_modal_select_piece();

    $(document).keyup(function (e) {
        if (e.which === 37) {
            $('#prev_move').addClass('active').focus().click();
            setTimeout("$('#prev_move').removeClass('active')", 300);
        }
        if (e.which === 39) {
            $('#next_move').addClass('active').focus().click();
            setTimeout("$('#next_move').removeClass('active')", 300);
        }
    });
});
function displayHistoryPgn() {
    var icons = {p: 'fa-chess-pawn', r: 'fa-chess-rook', n: 'fa-chess-knight', b: 'fa-chess-bishop', q: 'fa-chess-queen', k: 'fa-chess-king'};
    var history = game.history({verbose: true});
    var html = '';
    var line = 0;
    var pgnEl = $('#pgn');
    if (history.length > 0) {
        history.forEach(function (item, i) {
            var icon = icons[history[i].piece];
            if (i % 2 === 0) {
                line++;
                html += '<div class="line"><span>#' + line + ' </span><span class="pgn--' + item.color + '"><i class="fas ' + icon + '"></i> ' + item.to + '</span>';
            } else {
                html += '<span class="pgn--' + item.color + '"><i class="fas ' + icon + '"></i> ' + item.to + '</span></div>';
            }
        });
        pgnEl.html(html);
        pgnEl.scrollTop(pgnEl.get(0).scrollHeight);

    } else {
        pgnEl.html('');
    }
};
function getRemovedPiece(fen) {
    if (fen === undefined)
        fen = false;
    var pdef = {P: 8, R: 2, N: 2, B: 2, Q: 1, K: 1};
    var str = fen.split(' ')[0].replace(/\//g, '').replace(/[0-9]/g, '');
    var w = str.replace(/[a-z]/g, '');
    var b = str.replace(/[A-Z]/g, '');
    var pcurrent = {
        w: {P: m(w, 'P'), R: m(w, 'R'), N: m(w, 'N'), B: m(w, 'B'), Q: m(w, 'Q'), K: m(w, 'K')},
        b: {P: m(b, 'p'), R: m(b, 'r'), N: m(b, 'n'), B: m(b, 'b'), Q: m(b, 'q'), K: m(b, 'k')}
    };
    var pnew = {
        w: {P: 0, R: 0, N: 0, B: 0, Q: 0, K: 0},
        b: {P: 0, R: 0, N: 0, B: 0, Q: 0, K: 0}
    };
    function m(str, p) {
        var r = new RegExp(p, 'g')
        return (str.match(r)) ? str.match(r).length : 0;
    }
    ;
    for (key in pcurrent.b) {
        var count = pdef[key] - pcurrent.b[key];
        if (count > 0)
            pnew.b[key] = count;
    }
    ;
    for (key in pcurrent.w) {
        var count = pdef[key] - pcurrent.w[key];
        if (count > 0)
            pnew.w[key] = count;
    }
    ;
    showRemovedPiece(pnew);
};
function showRemovedPiece(pnew) {
    var whtml = '';
    var bhtml = '';
    var icons = {P: 'fa-chess-pawn', R: 'fa-chess-rook', N: 'fa-chess-knight', B: 'fa-chess-bishop', Q: 'fa-chess-queen', K: 'fa-chess-king'}
    for (key in pnew.w) {
        var uid = 'b' + key;
        if (pnew.w[key] > 0) {
            var html = '<i class="removed_piece_to_w fas ' + icons[key] + '"></i>';
            if (pnew.w[key] > 1)
                html = html.repeat(pnew.w[key]);
            whtml += '<span>' + html + '</span>';
        }
    }
    for (key in pnew.b) {
        var uid = 'w' + key;
        if (pnew.b[key] > 0) {
            var html = '<i class="removed_piece_to_b fas ' + icons[key] + '"></i>';
            if (pnew.b[key] > 1)
                html = html.repeat(pnew.b[key]);
            bhtml += '<span>' + html + '</span>';
        }
    }
    $('#removed_piece1').html((MyColor === 'b') ? bhtml : whtml);
    $('#removed_piece2').html((MyColor === 'b') ? whtml : bhtml);
};
var history_counter = false;
var prev_move = false;
var next_move = false;
function startMove() {
    var history_fen = game_history_fen;
    prev_move = 0;
    history_counter = 0;
    board.position(history_fen[0].split(' ')[0]);
};
function endMove() {
    var history_fen = game_history_fen;
    var len = history_fen.length;
    prev_move = true;
    history_counter = len - 1;
    board.position(history_fen[len - 1].split(' ')[0]);
};
function prevMove() {
    var history_fen = game_history_fen;
    var len = history_fen.length;
    if (prev_move === 0 || history_counter === 0)
        return;
    if (history_counter === false)
        history_counter = len - 1;
    prev_move = history_counter - 1;
    board.position(history_fen[prev_move].split(' ')[0]);
    history_counter--;
};
function nextMove() {
    var history_fen = game_history_fen;
    var len = history_fen.length;
    if (history_counter === len - 1 || history_counter === false)
        return;
    next_move = history_counter + 1;
    board.position(history_fen[next_move].split(' ')[0]);
    history_counter++;
    prev_move = true;
};
function mount_modal_select_piece() {
    var modal = $('#modal_select_piece_wrapper');

    if (!modal.length)
        return;

    var piece_theme = (localStorage['cb_piece']) ? localStorage['cb_piece'].split("--")[1] : 'tour_new';
    var piece_color = (typeof MyColor !== 'undefined') ? MyColor : 'w';

    modal.find('.btn___select_piece').each(function (ind, el) {
        var btn = $(el);
        var piece = btn.data('piece').toUpperCase();
        btn.html('<img src="/board/pieces/' + piece_theme + '/' + piece_color + piece + '.png">');
    });
};
function select_piece_promotion(move) {
    modal_open('select_piece');
    $('.btn___select_piece').on('click', function (e) {
        modal_close('select_piece');
        move['promotion'] = $(this).data('piece');
        makeMove(move);
        snapEnd();
    });
};
