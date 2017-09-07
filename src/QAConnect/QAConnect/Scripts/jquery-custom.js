$(window).on('click', (function (e) {
    window.onbeforeunload = null;
}));
$(window).load(function () {
    setTimeout(function () {
        twemoji.parse($("ul.emoji-list")[0], {
            size: 72,
            folder: 'svg',
            ext: '.svg'
        });
        twemoji.parse($("ul.emoji-list")[1], {
            size: 72,
            folder: 'svg',
            ext: '.svg'
        });
        twemoji.parse($("ul.emoji-list")[2], {
            size: 72,
            folder: 'svg',
            ext: '.svg'
        });
        twemoji.parse($("ul.emoji-list")[3], {
            size: 72,
            folder: 'svg',
            ext: '.svg'
        });
    }, 1000);


    if ($('.SaveProfileDetail')[0]) {
        var settings = $.data($('.SaveProfileDetail')[0], 'validator').settings;
        settings.submitHandler = function (form, event) {
            var $thisButton = $(form).find("button[type=submit]");
            $thisButton.button('loading');
            event.preventDefault();
            var tokenKey = 'accessToken';
            var _message = {};
            var token = sessionStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            var frmValues = $(form).serialize();
            frmValues += '&PageType=' + vmPage.PageType();
            frmValues += '&Id=' + vmPage.ID();
            return $.ajax({
                headers: headers,
                url: pageApiUrl + '/SavePageDetail/',
                data: frmValues,
                cache: true,
                processData: false,
                type: 'POST',
                success: function (data) {
                    $thisButton.button('reset');
                    vmMessage.newMessage({ 'Title': 'Profile detail has been saved successfully !!!', 'Type': MessageType.Success });
                }
            });
        };
    }
});
$(document).ready(function () {
    vmHeader.searchFriends();
    $(document).unbind('keydown.prettyphoto').bind('keydown.prettyphoto', function (e) {
        if (typeof $pp_pic_holder != 'undefined') {
            if ($pp_pic_holder.is(':visible')) {
                switch (e.keyCode) {
                    case 37: $.prettyPhoto.changePage('previous');
                        e.preventDefault();
                        break;
                    case 39:
                        $.prettyPhoto.changePage('next'); e.preventDefault();
                        break;
                    case 27: if (!settings.modal)
                        $.prettyPhoto.close();
                        e.preventDefault();
                        break;
                }
                ;
            };
        };
    });

    $('.feed_form li a').click(function () {
        $('.global_attachment_holder_section').hide();
        $('#' + $(this).attr('rel')).show();
        $('#global_attachment_status textarea').attr('placeholder', $.trim($(this).find('span').text()));
        $('.feed_form li a').removeClass('active');
        $(this).addClass('active');
        $('.activity_feed_form_button').show();
    })
    $('#global_attachment_status textarea').click(function () {
        $('.activity_feed_form_button').show();
    });

    $('.privacy').multiselect({
        dropRight: true,
        templates: {
            button: '<button type="button" class="btn btn-sm btn-inverse multiselect dropdown-toggle" data-toggle="dropdown"><i style="margin-right:5px;" class="fa fa-lock"></i><span class="caret"></span></button>',
        },
    });

    $('.privacy.dropdown-menu li').click(function () { });
    addNewPollOption = function () {
        $('.js_poll_feed_answer').append('<li> <input style="  width: 85%;" type="text" class="form-control input-sm" /></li>');
        return false;
    }

    $('button.navbar-toggle').click(function () {
        $('.left-pane').toggleClass('left-pane-open');
        //   $('.right-pane').toggleClass('right-pane-open');
    });
    $('.search-li').click(function () {
        $('.top-search').toggle();
    });

    $('.showModal').on('hidden.bs.modal', function (e) {
        $('.popup-modal .modal-dialog').html('');
    })

    var $root = $('html, body');
    $('body').on('click', '.hashtag', function () {
        var target = $(this.hash).offset();
        $root.animate({
            scrollTop: target ? target.top - 50 : window.height
        }, 500);
        return false;
    });

    //$('body').on('click', '.chat-window-emoji-btn', function () {
    //    twemoji.parse($("ul.emoji-list")[0], {
    //        size: 72,
    //        folder: 'svg',
    //        ext: '.svg'
    //    });
    //    twemoji.parse($("ul.emoji-list")[1], {
    //        size: 72,
    //        folder: 'svg',
    //        ext: '.svg'
    //    });
    //    twemoji.parse($("ul.emoji-list")[2], {
    //        size: 72,
    //        folder: 'svg',
    //        ext: '.svg'
    //    });
    //    twemoji.parse($("ul.emoji-list")[3], {
    //        size: 72,
    //        folder: 'svg',
    //        ext: '.svg'
    //    });
    //    $('body').off('click', '.chat-window-emoji-btn');
    //});
});
function adjustModalMaxHeightAndPosition() {
    $('.modal').each(function () {
        if ($(this).hasClass('in') === false) {
            $(this).show();
        }
        var contentHeight = $(window).height() - 60;
        var headerHeight = $(this).find('.modal-header').outerHeight() || 2;
        var footerHeight = $(this).find('.modal-footer').outerHeight() || 2;

        $(this).find('.modal-content').css({
            'max-height': function () {
                return contentHeight;
            }
        });

        $(this).find('.modal-body').css({
            'max-height': function () {
                return contentHeight - (headerHeight + footerHeight);
            }
        });

        $(this).find('.modal-dialog').addClass('modal-dialog-center').css({
            'margin-top': function () {
                return -($(this).outerHeight() / 2);
            }
            , 'margin-left': function () {
                return -($(this).outerWidth() / 2);
            }
        });
        if ($(this).hasClass('in') === false) {
            $(this).hide();
        }
    });
}
if ($(window).height() >= 320) {
    $(window).resize(adjustModalMaxHeightAndPosition).trigger("resize");
}

var flag = true;
//$(".chat-window-emoji-btn").click(
function loadEmoji() {
    debugger;
    if (flag) {
        //twemoji.parse(document.getElementsByClassName('emoji-list')[0], {
        //    size: 72,
        //    folder: 'svg',
        //    ext: '.svg'
        //});
        flag = false;
    }
    var container = $(document.getElementsByClassName('emoji-list')[0]);
    if (!container.hasClass('hidden')) {
        container.addClass('hidden');
        container.slideUp(500);
    }
    else {
        container.removeClass('hidden');
        container.slideUp(500);
    }
};