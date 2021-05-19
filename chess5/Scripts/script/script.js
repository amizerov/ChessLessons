$(document).ready(function () {

    function sidebar() {
        var m_pn = $('.m_sidebar_open'),
            m_cls = $('.m_sidebar_close'),
            s_clps = $('.sidebar-collapse'),
            wrp = $('#wrapper');

        m_pn.on('click', function () {
            wrp.removeClass('main-sidebar__closed').addClass('main-sidebar__opened');
        });
        m_cls.on('click', function () {
            wrp.removeClass('main-sidebar__opened').addClass('main-sidebar__closed');
        });

        s_clps.on('click', function () {
            if (wrp.hasClass('main-sidebar__collapsed')) {
                wrp.removeClass('main-sidebar__collapsed');
                document.cookie = "main_sidebar=default; path=/; expires" + new Date(new Date().getTime() + 60 * 1000);
            } else {
                wrp.addClass('main-sidebar__collapsed');
                document.cookie = "main_sidebar=collapsed; path=/; expires" + new Date(new Date().getTime() + 60 * 1000);
            }
            wrp.removeClass('main-sidebar__opened').addClass('main-sidebar__closed');
        });

    }

    function modal_login() {
        var m_l_wrp = $('#modal_login_wrapper'),
            m_cls = $('.modal__close'),
            m_l_pn = $('.modal_login_open'),
            m_r_lnk = $('.auth__register-link');

        m_l_pn.on('click', function () {
            m_l_wrp.addClass('active');
        });

        m_l_wrp.on('click', function () {
            //m_l_wrp.removeClass('active');
        });

        m_cls.on('click', function () {
            m_l_wrp.removeClass('active');
        });

        m_r_lnk.on('click', function () {
            m_l_wrp.removeClass('active');
            $('#modal_register_wrapper').addClass('active');
        });
    }

    function modal_register() {
        var m_r_wrp = $('#modal_register_wrapper'),
            m_cls = $('.modal__close'),
            m_r_pn = $('.modal_register_open');

        m_r_pn.on('click', function () {
            m_r_wrp.addClass('active');
        });

        m_r_wrp.on('click', function () {
            //m_r_wrp.removeClass('active');
        });

        m_cls.on('click', function () {
            m_r_wrp.removeClass('active');
        });
    }

    function modal_close() {
        var m_cls = $('.modal__close');
        m_cls.on('click', function () {
            $('.modal-wrapper').removeClass('active');
        })
    }

    function modal_collapse() {
        var m_cps = $('.modal__collapse');
        var m = m_cps.parent().parent();
        var t = m_cps.attr('data-text-collapsed');
        var f = m_cps.attr('data-func-collapsed');
        m_cps.on('click', function () {
            m.removeClass('active');
            $('body').append('<div class="panel-modal-collapsed"><div id="collapsed__'+ f +'_modal" class="item" onclick="modal_open(\''+ f +'\')">'
                +'<img src="https://img.icons8.com/windows/22/000000/back.png"><span>'+ t +'</span></div></div>');
        })
    }
    
    function adaptive_sidebar() {
        var w = $(window).width(),
            c = (typeof (getCookie) === 'function') ? getCookie('main_sidebar') : '';
        if (w < 1200 && w > 991) {
            $('#wrapper').addClass('main-sidebar__collapsed');
        } else if (w > 1200 && (!c || c === 'default')) {
            $('#wrapper').removeClass('main-sidebar__collapsed');
        }
    }

    function active_nav_item() {
        var get_r = $("meta[name='get_request']").attr('content');

        $('.main-sidebar a[data-request]').each(function (ind, e) {
            if ($(e).attr('data-request') === get_r) {
                $(e).addClass('current_link');
            }
        });
    }

    function adaptive_content_panel() {

        $('#show_panel_wrp_1').on('click', function () {
            if ($('#panel_wrp_1').hasClass('active')) {
                $('#panel_wrp_1').removeClass('active');
            } else {
                $('#panel_wrp_2').removeClass('active');
                $('#panel_wrp_3').removeClass('active');
                $('#panel_wrp_1').addClass('active');
            }
        });
        $('#panel_wrp_1 .panel__close').on('click', function () {
            $('#panel_wrp_1').removeClass('active');
        });

        $('#show_panel_wrp_2').on('click', function () {
            if ($('#panel_wrp_2').hasClass('active')) {
                $('#panel_wrp_2').removeClass('active');
            } else {
                $('#panel_wrp_1').removeClass('active');
                $('#panel_wrp_3').removeClass('active');
                $('#panel_wrp_2').addClass('active');
            }
        });
        $('#panel_wrp_2 .panel__close').on('click', function () {
            $('#panel_wrp_2').removeClass('active');
        });
        $('#show_panel_wrp_3').on('click', function () {
            if ($('#panel_wrp_3').hasClass('active')) {
                $('#panel_wrp_3').removeClass('active');
            } else {
                $('#panel_wrp_1').removeClass('active');
                $('#panel_wrp_2').removeClass('active');
                $('#panel_wrp_3').addClass('active');
            }
        });
        $('#panel_wrp_3 .panel__close').on('click', function () {
            $('#panel_wrp_3').removeClass('active');
        });
    }

    function content_panel_scroll() {
        $('.panel__scroll-wrapper').each(function (ind, e) {
            if ($(e).height() > $(e).parent().height()) {
                $(e).parent().addClass('panel_has_scroll');
            }
        });
    }

    function chess_board_resize() {
        if ($('#board').length > 0) {
            if (typeof (board.resize) === 'function') {
                setSize();
                board.resize();
            } else if (typeof (game.boardResize) === 'function') {
                setSize();
                game.boardResize();
            }
        }

        function setSize() {
            var w = $(window).width(),
                h = $(window).height();

            var cntnt_t = $('.content__title').length > 0 ? $('.content__title').height() + 12 : 0;
            var new_w = h - ($('.m_header').height() + $('.main-footer').height() + cntnt_t + 55);

            if (w < 992) {
                $('#board').css('width', new_w);
            } else {
                if($('.board-top').length > 0) {
                    $('#board').css('width', h - 200 );//h-100
                } else {
                    //console.log(h - 60);
                    $('#board').css('width', h - 120 );//h-60
                }
            }
            
        }
    }

    sidebar();
    modal_login();
    modal_register();
    modal_close();
    modal_collapse();
    adaptive_sidebar();
    active_nav_item();
    adaptive_content_panel();
    chess_board_resize();
    //content_panel_scroll();

    $(window).resize(function () {
        adaptive_sidebar();
        chess_board_resize();
        //content_panel_scroll();
    });

}); 

function modal_open(n) {
    $('#modal_' + n + '_wrapper').addClass('active');
    if($('#collapsed__'+n+'_modal').length > 0) {
        $('#collapsed__'+n+'_modal').parent().remove();
    }
}

function modal_close(n) {
    $('#modal_' + n + '_wrapper').removeClass('active');
}
/*
function setBoardPiece(piece) {
    if(piece === undefined)
        piece = 'cb_piece--tour_new';

    var body = $('body');
    var body_clss = body.attr('class');
    
    if(typeof body_clss !== 'undefined' && body_clss.length > 0)
        body.removeClass(['cb_piece--tour_new', 'cb_piece--chess24', 'cb_piece--wikipedia', 'cb_piece--metro']);
    body.addClass(piece);

    piece = piece.split("--")[1];
    if($('.cb_chessboard').length <= 0)
        return;
    $('.cb_piece').each( function(ind, el){
        $(el).attr('src', function(ind, attr){
            return attr.replace(/\.\/board\/pieces\/.*\//gi, './board/pieces/'+piece+'/');
        });
    });
    game.setPieceTheme('./board/pieces/'+piece+'/{piece}.png');
}
function setBoardColor(color) {
    if(color === undefined)
        color = 'default';
    
    //localStorage['cb_color'] = color;
    color = 'cb_color--' + color;
    
    var body = $('body');
    var body_clss = body.attr('class');
    
    if(typeof body_clss !== 'undefined' && body_clss.length > 0)
        body.removeClass(['cb_color--default', 'cb_color--blue', 'cb_color--green', 'cb_color--grey', 'cb_color--wood1', 'cb_color--wood2']);
    
    body.addClass(color);
}

function setSiteColor(color) {
    if(color === undefined)
        color = 'default';
    
    localStorage['site_color'] = color;
    color = 'site_color--' + color;
    
    var body = $('body');
    var body_clss = body.attr('class');
    
    if(typeof body_clss !== 'undefined' && body_clss.length > 0)
        body.removeClass(['site_color--default', 'site_color--blue', 'site_color--green', 'site_color--grey']);
    
    body.addClass(color);
}

function setSiteBg(bg) {
    if(bg === undefined)
        bg = 'default';
    
    localStorage['site_bg'] = bg;
    bg = 'site_bg--' + bg;
    
    var body = $('body');
    var body_clss = body.attr('class');
    
    if(typeof body_clss !== 'undefined' && body_clss.length > 0)
        body.removeClass(['site_bg--default', 'site_bg--1', 'site_bg--2', 'site_bg--3', 'site_bg--4', 'site_bg--5']);
    
    body.addClass(bg);
}
*/

$('.nav-tabs li').click(function () {
    $('.nav-tabs li').removeClass('active');
    $(this).addClass('active');
});


// modal settingss

const settingsColorPanel = document.querySelector('.set_color');
const settingsBackgroundPanel = document.querySelector('.set_bg');
const settingsBoardPanel = document.querySelector('.set_board');
const settingsFigurePanel = document.querySelector('.set_figure');

function setSettingsDefault() {

    localStorage['site_color']  = settingsSiteDef.site_color;
    localStorage['site_bg']     = settingsSiteDef.site_bg;
    localStorage['cb_color']    = settingsSiteDef.cb_color;
    localStorage['cb_piece']    = settingsSiteDef.cb_piece;
    
    var body = $('body');
    var body_clss = body.attr('class');
    
    if(typeof body_clss !== 'undefined' && body_clss.length > 0) {
        body.removeClass(settingsSiteVals.site_color);
        body.removeClass(settingsSiteVals.site_bg);
        body.removeClass(['cb_piece--tour_new', 'cb_piece--chess24', 'cb_piece--wikipedia', 'cb_piece--metro']);
        body.removeClass(settingsSiteVals.cb_color);
    }
    
    body.addClass([
        settingsSiteDef.site_color,
        settingsSiteDef.site_bg,
        settingsSiteDef.cb_piece,
        settingsSiteDef.cb_color
    ]);
    setSavedValues();
    setBoardPiece(undefined);
}

function setSavedValues() {
    
    if (localStorage['site_color']) {
        settingsColorPanel.querySelector(`.${localStorage['site_color']}`).classList.add('active');
        $('body').removeClass(settingsSiteVals.site_color);
        $('body').addClass(localStorage['site_color']);
    }

    if (localStorage['cb_color']){
        settingsBoardPanel.querySelector(`.${localStorage['cb_color']}`).classList.add('active');
        $('body').removeClass(settingsSiteVals.cb_color);
        $('body').addClass(localStorage['cb_color']);
    }

    if (localStorage['site_bg']){
        settingsBackgroundPanel.querySelector(`.${localStorage['site_bg']}`).classList.add('active');
        $('body').removeClass(settingsSiteVals.site_bg);
        $('body').addClass(localStorage['site_bg']);
    }

    if (localStorage['cb_piece']){
        settingsFigurePanel.querySelector(`.${localStorage['cb_piece']}`).classList.add('active');
        $('body').removeClass(settingsSiteVals.cb_piece);
        $('body').addClass(localStorage['cb_piece']);
    }

}
setSavedValues();

settingsColorPanel.addEventListener('click', function(e) {
    const items = settingsColorPanel.querySelectorAll('button');
    const target = e.target;
    Array.from(items).forEach(item => {
        item.classList.remove('active');
    });
    target.classList.add('active');
    $('body').removeClass(settingsSiteVals.site_color);
    $('body').addClass(target.classList[0]);
});

settingsBackgroundPanel.addEventListener('click', function(e) {
    const items = settingsBackgroundPanel.querySelectorAll('button');
    const target = e.target;
    Array.from(items).forEach(item => {
        item.classList.remove('active');
    });
    target.classList.add('active');
    $('body').removeClass(settingsSiteVals.site_bg);
    $('body').addClass(target.classList[0]);
});

settingsBoardPanel.addEventListener('click', function(e) {
    const items = settingsBoardPanel.querySelectorAll('button');
    const target = e.target;
    Array.from(items).forEach(item => {
        item.classList.remove('active');
    });
    target.classList.add('active');
    $('body').removeClass(settingsSiteVals.cb_color);
    $('body').addClass(target.classList[0]);
});

$(".set_figure button").bind('click', function(e) {
    $(".set_figure button").removeClass('active');
    $(this).addClass('active');

    let activeClass = $(this).attr('class').split(' ')[0];

    $('body').removeClass(settingsSiteVals.cb_piece);
    $('body').addClass(activeClass);
    
    var piece = activeClass.split("--")[1];
    
    if($('.cb_chessboard').length <= 0)
        return;
    
    $('.cb_piece').each( function(ind, el){
        $(el).attr('src', function(ind, attr){
            return attr.replace(/\.\/board\/pieces\/.*\//gi, './board/pieces/'+piece+'/');
        });
    });
    
    if(typeof game === 'object' && typeof game.setPieceTheme === 'function')
        game.setPieceTheme('./board/pieces/'+piece+'/{piece}.png');
});


let acceptBtn = document.querySelector('.accept__settings');
let cancelBtn = document.querySelector('.cancel__settings');
let cancelBtn2 = document.querySelector('.cancel__settings2');

acceptBtn.addEventListener('click',function () {
    let setColor = settingsColorPanel.querySelector('.active').classList[0];
    let setBg = settingsBackgroundPanel.querySelector('.active').classList[0];
    let setBoard = settingsBoardPanel.querySelector('.active').classList[0];
    let setFigure = settingsFigurePanel.querySelector('.active').classList[0]

    localStorage['site_bg'] = setBg;
    localStorage['site_color'] = setColor;
    localStorage['cb_color'] = setBoard;
    localStorage['cb_piece'] = setFigure;
    setSavedValues();
    $('.modal-wrapper').removeClass('active');
});

cancelBtn.addEventListener('click',function () {
    setSavedValues();
    $('.modal-wrapper').removeClass('active');
});

cancelBtn2.addEventListener('click',function () {
    setSavedValues();
    $('.modal-wrapper').removeClass('active');
});

$(document).keyup(function(e) {
    if (e.keyCode === 27) {
        setSavedValues();
        $('.modal-wrapper').removeClass('active');
    }
});


/* notes */
!function(d) {
	"use strict";
	Object.assign||Object.defineProperty(
		Object,"assign",{
			enumerable:!1,configurable:!0,writable:!0,value:function(e,r){
				"use strict";
				if(null==e)throw new TypeError("Cannot convert first argument to object");
				for(var t=Object(e),n=1;n<arguments.length;n++){
					var o=arguments[n];
					if(null!=o)for(var a=Object.keys(Object(o)),c=0,b=a.length;c<b;c++){
						var i=a[c],l=Object.getOwnPropertyDescriptor(o,i);void 0!==l&&l.enumerable&&(t[i]=o[i])
					}
				}
				return t
			}
		}
	);
	"remove" in Element.prototype||(Element.prototype.remove=function(){this.parentNode&&this.parentNode.removeChild(this)});

	window.note = function(settings) {

		settings = Object.assign({},{
			callback:    false,
			content:     "",
			time:        4.5,
			type:        "info"
		}, settings);

		if(!settings.content.length) return;

		var create = function(name, attr, append, content) {
			var node = d.createElement(name);
			for(var val in attr) { if(attr.hasOwnProperty(val)) node.setAttribute(val, attr[val]); }
			if(content) node.insertAdjacentHTML("afterbegin", content);
			append.appendChild(node);
			if(node.classList.contains("note-item-hidden")) node.classList.remove("note-item-hidden");
			return node;
		};

		var noteBox = d.getElementById("notes") || create("div", { "id": "notes" }, d.body);
		var noteItem = create("div", {
				"class": "note-item",
                "id": "alert_games_1",
                "style": "width: 310px;min-width: 310px;",
				"data-show": "false",
				"role": "alert",
				"data-type": settings.type
			}, noteBox),
			noteItemText = create("div", { "class": "note-item-text" }, noteItem, settings.content),
			noteItemBtn = create("button", {
				"class": "note-item-btn alert_games_11",
				"type": "button",
				"aria-label": "Скрыть"
			}, noteItem);

		var isVisible = function() {
			var coords = noteItem.getBoundingClientRect();
			return (
				coords.top >= 0 &&
				coords.left >= 0 &&
				coords.bottom <= (window.innerHeight || d.documentElement.clientHeight) && 
				coords.right <= (window.innerWidth || d.documentElement.clientWidth) 
			);
		};

		var remove = function(el) {
			el = el || noteItem;
			el.setAttribute("data-show","false");
			window.setTimeout(function() {
				el.remove();
			}, 250);
			if(settings.callback) settings.callback();
		};

		noteItemBtn.addEventListener("click", function() { remove(); });

		window.setTimeout(function() {
		  noteItem.setAttribute("data-show","true");
		}, 250);

		if(!isVisible()) remove(noteBox.firstChild);

		window.setTimeout(remove, settings.time * 1000);

	};

}(document);
