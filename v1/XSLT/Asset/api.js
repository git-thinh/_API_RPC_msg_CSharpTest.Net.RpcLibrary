var m_sessionID = 'sessionid';

var eMSG =
{
    LOGIN : "LOGIN"
}

var m_split_controlAction = '___';

var msLAYER =
{
    NONE: 0,
    API: 1000000,
    PING_NONE: 1000001,
    PING_TIME: 1000002,
    FILE_UPLOAD: 1000003,
    FILE_UPLOAD_IMPORT: 1000004,
    API_CONFIG: 1000005,
    API_CONFIG_POUP: 1000006,
    MAIN: 2000000,
    POPUP_MAIN: 3000000,
    POPUP_SUB: 4000000
};

var msLAYOUT =
{
    TOP: 100000,
    LEFT: 200000,
    RIGHT: 300000,
    MAIN: 400000,
    PREVIEW: 500000,
    BOTTOM: 600000,
    POPUP_MAIN: 700000,
    POPUP_SUB: 800000,
};

var msCONTROL =
{
    TOOLBAR: 10000,
    SIDEBAR: 20000,
    TAB: 30000,
    GRID: 40000,
    FORM: 50000,
};

; (function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        define([], factory);
    } else if (typeof exports === 'object') {
        module.exports = factory();
    } else {
        root.api = factory();
    }
}(this, function () {

    // #region [ VAR ]

    var _timeOut_scroll_call_ajax = 500;
    var _timeOut_load_item_show = 500;

    var _typingTimer;
    var _doneTypingInterval = 700;

    var _ajax = new XMLHttpRequest();
    var _pageSize = 20;

    var _css_input_items = 'cob_iapiTC';

    var _css_button_arrow = 'cob_iapiTR';
    var _css_popup_items = 'cob_iapiIC';
    var _css_list_items = 'cobul';

    var _id_loading = '___loading';
    var _id_footer = '___footer';

    var _id_pagenumber = '___pagenumber';
    var _id_total_items = '___total_items';
    var _id_footer_show = '___footer_show';

    var _id_input = '___input_text';

    var _id_item_selected_id = '_id_item_selected_id';
    var _id_item_selected_text = '_id_item_selected_text';

    var _id_input_status = '___input_status';
    var _id_input_search = '___input_search';

    var _id_input_parent_id = '___input_parent_id';

    var _id_arrow = '___arrow';
    var _id_popup_items = '___popup_items';
    var _id_list_items = '___list_items';

    //#endregion

    if ("WebSocket" in window) {
        var uri_ws = 'ws://' + window.location.host; 
        var ws = new WebSocket(uri_ws);

        ws.onopen = function () {
            var sid = api.get_cookie(m_sessionID);
            ws.send(sid);
        };

        ws.onmessage = function (evt) {
            var data = evt.data;
            api.msg(data);
        };

        ws.onclose = function () {
            api.log("Connection is closed...");
        };
    }
    else {
        alert("WebSocket NOT supported by your Browser!");
    }

    var api = {
        msg: function (data) {
            api.log(data);
            switch (data) {
                case eMSG.LOGIN:
                    api.popup_login();
                    break;
            }
        },
        //===================================================
        logout: function () {
            //sessionStorage['user.username'] = dt[0]['username'];
            //sessionStorage['user.password'] = dt[0]['password'];
            var url = location.href.toString();
            location.href = url;
        },
        //===================================================
        get_number_min_max: function (min, max) {
            return (Math.floor(Math.random() * (max - min + 1)) + min).toString();
        },
        get_number_id: function (min, max) {
            var date = new Date();
            var id = date.getFullYear() + ("0" +
                (date.getMonth() + 1)).slice(-2) +
                ("0" + date.getDate()).slice(-2) +
                ("0" + date.getHours() + 1).slice(-2) +
                ("0" + date.getMinutes()).slice(-2) +
                ("0" + date.getSeconds()).slice(-2) +
                date.getMilliseconds();

            if (max != null && min != null)
                id += api.get_number_min_max(min, max);

            console.log(id);
            return id;
        },

        get_browser_height: function () {
            var browser_height = 0;
            if (document.body && document.body.offsetHeight) browser_height = document.body.offsetHeight;
            if (document.compatMode == 'CSS1Compat'
                && document.documentElement && document.documentElement.offsetHeight) browser_height = document.documentElement.offsetHeight;
            if (window.innerHeight && window.innerHeight) browser_height = window.innerHeight;
            return browser_height;
        },
        get_browser_width: function () {
            var browser_width = 0;
            if (document.body && document.body.offsetWidth) browser_width = document.body.offsetWidth;
            if (document.compatMode == 'CSS1Compat'
                && document.documentElement && document.documentElement.offsetWidth) browser_width = document.documentElement.offsetWidth;
            if (window.innerWidth && window.innerHeight) browser_width = window.innerWidth;
            return browser_width;
        },

        file_input_change: function (input) {
            var val = $('#file_excel').val()

            api.log(val);


            //api.log(input.value);

            //var url = input.value;
            //var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
            //if (input.files && input.files[0] && (ext == "xls" || ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
            //    var reader = new FileReader();

            //    reader.onload = function (e) {
            //        var data = e.target.result;

            //        //$('#img').attr('src', e.target.result);
            //        api.log(data);
            //    }
            //    reader.readAsDataURL(input.files[0]);
            //}
            //else {
            //    //$('#img').attr('src', '/assets/no_preview.png');
            //}
        },
        //===================================================
        popup: function (op) {
            //if (api.popup_allow() == false) return;

            var iz = $('.popup___').size();
            var bg = ' background-color: rgb(0,0,0); background-color: rgba(0,0,0,0.4);';
            if (iz != 0 && op != null && op['bg'] == false) bg = '';

            var b_close = '';
            if (op != null && op['closeshow'] != false) b_close = ' <span class="close___" style="float:right;font-size:28px;font-weight:bold;cursor:pointer;">×</span> ';


            var pop_name = '';
            if (op != null && op['name'] != null) pop_name = ' name="' + op['name'] + '" ';

            var hi = '';
            if (op != null && op['height'] != null) hi = 'height:' + op['height'] + 'px';

            var htm = 'HI', sw = '';
            if (iz == 0) iz = 999; else iz = '999' + (iz + 9).toString();

            var sw = 'width:450px;min-width:300px;';
            //var pw = htm.indexOf('width');
            //if (pw != -1) {
            //    var sw = htm.substr(pw + 5, 10);
            //    var wi = parseInt(sw.split('px')[0].split(':').join('').trim());
            //    if (wi > 0) {
            //        sw = 'width:' + (wi + 2) + 'px;';
            //    }
            //}

            if (op != null && op['width'] != null) sw = 'width:' + op['width'] + 'px;min-width:300px;';
            if (op != null && op['height'] != null) sw += ';height:' + (op['height'] + 30) + 'px;min-height:200px;';

            //$('.popup___').remove();

            var pid___ = 'pop_' + 'xxxxxxxx_xxxx_4xxx_yxxx_xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
            var tit = '', b = '';
            if (op != null && op['title'] != null) tit = '<h2 class="p_title___" style="padding: 7px 0px;margin: 0px;">' + op['title'] + '</h2>';
            //if (op != null && op['execute'] != null) {
            //    execute___f = op['execute'];
            //    if (op["footer"] != false)
            //        b = '<div style="display:inline-block;position: relative;width: 100%;padding:3px 16px;"><p style="text-align:center;"><button for="execute___" class="cyan_btn" style="margin-right:5px;">THỰC HIỆN</button><button class="close___ cyan_btn">ĐÓNG</button></p></div></h2>';
            //}

            var hStyle = 'border-radius: 0 0 0 0;background-image: -webkit-linear-gradient(#ececec,#dfdfdf);background-image: -moz-linear-gradient(#ececec,#dfdfdf);background-image: -ms-linear-gradient(#ececec,#dfdfdf);background-image: -o-linear-gradient(#ececec,#dfdfdf);background-image: linear-gradient(#ececec,#dfdfdf);filter: progid:dximagetransform.microsoft.gradient(startColorstr=\'#ffececec\', endColorstr=\'#ffdfdfdf\', GradientType=0);border-bottom: 1px solid #bfbfbf;';

            var sm =
                '<div for="modal-bg" class="popup___" id="' + pid___ + '" ' + pop_name + 'style="display:inline-block;position: fixed;z-index:' + iz + '; padding-top: 100px; left: 0;top: 0;width: 100%;height: 100%;overflow: auto;' + bg + ' ">' +
                '<div for="modal-content" data-draggable="true" id="' + pid___ + '_content" style="' + sw + 'opacity:0;display:inline-block;position:absolute;background-color:#fefefe;padding: 0 0 5px 0;border-radius:1px;border: 1px solid #888;box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);-webkit-animation-name: animatetop;-webkit-animation-duration: 0.4s;animation-name: animatetop;animation-duration: 0.4s;">' +
                '<div for="modal-header" style="display:inline-block;position:relative;width: calc(100% - 14px);padding:0px 4px 0 10px;color: #333;' + hStyle + '">' + b_close + tit + '</div>' +
                '<div for="modal-body" id="' + pid___ + '_body" style="display:inline-block;position:relative;width: 100%;min-height:69px;' + hi + '"><div class="popup_layout___" style="position: absolute;width: 100%;height: 100%;background: silver;">' + htm + '</div></div>' + b +
                '</div></div>';

            sm = sm.split('for="execute___"').join('onclick="popup_load_execute_f(\'' + pid___ + '\')"');

            $(document.body).append(sm).promise().done(function () {
                $(document.body).css({ 'overflow': 'hidden' });

                setTimeout(function () {
                    // FIX width, height POPUP --------------------------------------------------------
                    var w = $('#' + pid___ + '_content').width(), h = $('#' + pid___ + '_content').height();
                    //console.log(w + ';' + h);
                    //console.log(browser_width + ',' + browser_height);
                    var l = parseInt((api.get_browser_width() - w) / 2), t = parseInt((api.get_browser_height() - h) / 2);
                    if (t < 0) t = 0;
                    if (l < 0) l = 0;
                    $('#' + pid___ + '_content').css({ 'left': l + 'px', 'top': t + 'px', 'opacity': 1 });

                    // LAYOUT POPUP --------------------------------------------------------
                    if (op != null) {
                        var v_form = op['form'], v_sidebar = op['sidebar'], v_grid = op['grid'];
                        if (v_form != null || v_sidebar != null || v_grid != null) {
                            var p_hide_left = true;
                            if (v_sidebar != null) p_hide_left = false;

                            //alert('form');
                            var mp_layout = pid___ + '_layout';
                            $('#' + pid___ + '_content .popup_layout___').w2layout({
                                name: mp_layout,
                                panels: [
                                    { type: 'top', size: 30, hidden: true },
                                    { type: 'left', size: '20%', resizable: true, hidden: p_hide_left },
                                    { type: 'main', content: '<div id="' + pid___ + '_layout_main"></div>' },
                                    { type: 'right', size: 150, resizable: true, hidden: true },
                                    { type: 'bottom', size: 30, resizable: true, hidden: true },
                                ]
                            });

                            if (v_form != null) {
                                //v_form['name'] = pid___ + '_form';
                                //w2ui[mp_layout].content('main', $().w2form(v_form));
                                //$('#' + pid___ + '_layout_main').w2form(v_form);

                                //setTimeout(function () {
                                //    var hs = $('.popup___ .w2ui-page').height();
                                //    $('#' + pid___ + '_body').css({ height: hs + 70 + 'px' });
                                //    $('.popup___ .w2ui-page').slimscroll({
                                //        start: 'top', position: 'right', size: '3px', distance: '0px', alwaysVisible: true, railVisible: true,
                                //        height: hs + 'px', color: '#333', railColor: '#fff', borderRadius: 4, railBorderRadius: 0, railDraggable: true
                                //    });
                                //    $('.popup___ .w2ui-page').focus();
                                //}, 10);

                                var fid_ = v_form['name']; //alert(fid_);
                                if (w2ui[fid_] == null)
                                    w2ui[mp_layout].content('main', $().w2form(v_form));
                                else
                                    w2ui[mp_layout].content('main', w2ui[fid_]);

                            } else if (v_grid != null) {
                                //w2ui[mp_layout].toggle('bottom', window.instant);
                                var gid_ = v_grid['name']; //alert(gid_);
                                if (w2ui[gid_] == null)
                                    w2ui[mp_layout].content('main', $().w2grid(v_grid));
                                else
                                    w2ui[mp_layout].content('main', w2ui[gid_]);
                            }

                            $('#' + pid___ + '_content').tinyDraggable({ handle: '.p_title___' });


                        } //end w2ui
                    }

                    // EVENT OPEN POPUP --------------------------------------------------------
                    if (op != null && op['open'] != null) op.open(pid___);
                }, 300);

                setTimeout(function () {
                    //$('#' + pid___ + ' .execute___').on('click', function () {
                    //$('#' + pid___ + ' .execute___').click(function () {
                    //    if (op != null && op['execute'] != null) op.execute(pid___);
                    //});

                    $('#' + pid___ + ' .close___').on('click', function () {

                        $(document.body).css({ 'overflow': 'auto' });

                        if (op != null && op['close'] != null) {
                            op.close(pid___);
                            $('#' + pid___).hide();
                            setTimeout(function () {
                                $('#' + pid___).remove();
                            }, 1000);;
                        } else {
                            $('#' + pid___).remove();
                        }
                    });
                }, 1000);
            });

            //close-modal 

        },
        popup_allow: function () {
            var login = api.get_cookie('login_open');
            if (login == 'opening') return false;
            return true;
        },


        popup_login_close: function () {
            $('.popup___').remove();
        },
        popup_login: function (msg) {
            if (msg == null) msg = '';
            var _pop_allow = false;
            var its = document.getElementsByClassName('popup___');
            var pop_count = its.length;
            if (pop_count > 0) {
                for (var k = 0; k < its.length; k++) {
                    var nam = its[k].getAttribute('name');
                    if (nam == 'popup_login')
                        return;
                }
                _pop_allow = true;
            }
            else {
                _pop_allow = true;
            }

            if (_pop_allow) {
                var frm_name = api.get_number_id();
                var cf_login_options = {
                    name: 'popup_login',
                    height: 180,
                    title: 'System Login',
                    bg: false,
                    closeshow: false,
                    close: function (pid) { },
                    open: function (pid) {
                        api.log(pid);
                        //$('#' + pid + ' button[name=Save]').val('Login');
                        //$('#' + pid + ' button[name=Clear]').val('Reset');
                        $('#' + pid + ' .w2ui-buttons').html('<button name="Save" class="w2ui-btn w2ui-btn-blue" style="">Login</button><button name="Clear" class="w2ui-btn " style="">Reset</button>');
                    },
                    form: {
                        name: frm_name,
                        overflow: 'hidden',
                        fields: [
                            { field: 'username', type: 'text', required: true, html: { caption: 'Username', attr: ' style="width: 200px"' } },
                            { field: 'password', type: 'password', required: true, html: { caption: 'Password', attr: ' style="width: 200px"' } }
                        ],
                        record: {
                            username: 'admin',
                            password: 'admin'
                        },
                        actions: {
                            'Save': function (event) {
                                var fom = w2ui[frm_name].record;
                                console.log(fom);
                                //this.save();

                                var username = fom['username'];
                                var password = fom['password'];
                                console.log(username + '=' + password);

                                //this.save(function (rsa) {
                                //    if (rsa.status == 'success') {
                                //        //w2ui['users'].reload();
                                //        //$().w2popup('close');
                                //        console.log(rsa);
                                //    }
                                //    // if error, it is already displayed by w2form
                                //});

                                ////var fd = {
                                ////    'lock': 'true',
                                ////    'control_redirect': 'layout',
                                ////    'action_redirect': 'main',
                                ////    'username': username,
                                ////    'password': password
                                ////};

                                ////api.api_post(msLAYER.API_CONFIG, 'user', 'login', fd, {
                                ////    ok: function (val) {
                                ////        //var pid = $(event.target).closest('.popup___').attr('id');
                                ////        //$('#' + pid + ' .close___').click();
                                ////        api.api_lock();
                                ////    }
                                ////});

                                var data = JSON.stringify(fom);
                                console.log(data);
                                //var b64 = window.btoa(data);
                                api.ajax(data, '/login', {
                                    ok: function (val) {
                                        //var pid = $(event.target).closest('.popup___').attr('id');
                                        //$('#' + pid + ' .close___').click();
                                        api.api_lock();
                                    }
                                });
                            },
                            'Clear': function (event) {
                                console.log('clear', event);
                                this.clear();
                            },
                        }
                    }
                };

                api.popup(cf_login_options);
            }
        },
        login_check: function () {
            var sid = api.get_sessionid();
            if (sid == null || sid.indexOf('1900') == 0) {
                api.popup_login('');
                setTimeout(api.login_check, 300);
            }
        },
        //===================================================

        get_sessionid: function () {
            var sid = api.get_cookie('sessionid');
            return sid;
        },

        get_closest: function (el, selector) {
            // get_closest(node,'.cobs')

            var matchesFn;
            // find vendor prefix
            ['matches', 'webkitMatchesSelector', 'mozMatchesSelector', 'msMatchesSelector', 'oMatchesSelector'].some(function (fn) {
                if (typeof document.body[fn] == 'function') {
                    matchesFn = fn;
                    return true;
                }
                return false;
            })

            if (el[matchesFn](selector)) return el;
            var parent;
            // traverse parents
            while (el) {
                parent = el.parentElement;
                if (parent && parent[matchesFn](selector)) {
                    return parent;
                }
                el = parent;
            }
            return null;
        },
        get_page_path: function () {
            var path = location.pathname;
            var a = path.split('/');
            var page = a[a.length - 1].toLowerCase().split('.aspx').join('');
            if (page == '' || page == 'default' || page == 'home') page = 'index';
            //api.log(page);
            return page;
        },
        get_cookie: function (cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        },
        set_cookie: function (cname, cvalue) {
            var exdays = 10; // set cookies 10 day
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        },

        get_node: function (id) { return document.getElementById(id); },

        get_xpath: function (node) {
            //Gets an XPath for an node which describes its hierarchical location.

            var comp, comps = [];
            var parent = null;
            var xpath = '';
            var getPos = function (node) {
                var position = 1, curNode;
                if (node.nodeType == Node.ATTRIBUTE_NODE) {
                    return null;
                }
                for (curNode = node.previousSibling; curNode; curNode = curNode.previousSibling) {
                    if (curNode.nodeName == node.nodeName) {
                        ++position;
                    }
                }
                return position;
            }

            if (node instanceof Document) {
                return '/';
            }

            for (; node && !(node instanceof Document) ; node = node.nodeType == Node.ATTRIBUTE_NODE ? node.ownerElement : node.parentNode) {
                comp = comps[comps.length] = {};
                switch (node.nodeType) {
                    case Node.TEXT_NODE:
                        comp.name = 'text()';
                        break;
                    case Node.ATTRIBUTE_NODE:
                        comp.name = '@' + node.nodeName;
                        break;
                    case Node.PROCESSING_INSTRUCTION_NODE:
                        comp.name = 'processing-instruction()';
                        break;
                    case Node.COMMENT_NODE:
                        comp.name = 'comment()';
                        break;
                    case Node.ELEMENT_NODE:
                        comp.name = node.nodeName;
                        break;
                }
                comp.position = getPos(node);
            }

            for (var i = comps.length - 1; i >= 0; i--) {
                comp = comps[i];
                var name = comp.name;
                if (name != 'HTML' && name != 'BODY') {
                    //xpath += '/' + comp.name;
                    if (xpath == '') xpath = name; else xpath += '__' + name;
                    if (comp.position != null) {
                        //xpath += '[' + comp.position + ']';
                        xpath += '_' + comp.position;
                    }
                }
            }

            return xpath.toLowerCase();
        },

        guid: function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8;
                return v.toString(8);
            });
        },

        log: function (data) {
            console.log(data);
        },

        ajax: function (data, url, event) {
            //url = '/ajax.ashx';
            api.log('ajax -> ' + url);
            //var data = 'text=' + text + '&pagesize=' + pageSize + '&pagenumber=' + pageNumber;
            _ajax = new XMLHttpRequest();
            _ajax.onreadystatechange = function () {
                if (_ajax.readyState == 4 && _ajax.status == 200) {
                    if (event != null && event.ok != null) event.ok(_ajax.responseText);
                }
            };
            _ajax.addEventListener('error', function (event) {
                if (event != null && event.error != null) event.error();
            });

            _ajax.open("POST", url, true);
            _ajax.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            _ajax.send(data);
        },

        get_api_array_all_id: function () {
            var s = api.get_cookie('api_all_id');
            if (s != null) {
                var a = s.split('|');
                return a;
            }
            return new Array();
        },
        get_id: function (name) {
            var page_path = api.get_page_path();
            var id = api.get_cookie(page_path + '_' + name);
            api.log('get_id ->' + id + ' = ' + name);
            return id;
        },
        get_name: function (id) {
            var it = document.getElementById(id);
            if (it != null) {
                var name = it.getAttribute('name');
                api.log('get_name ->' + id + ' = ' + name);
                return name;
            }
            return null;
        },

        get_page_size: function () {
            var psize = 0;
            var s = api.get_cookie('api_page_size');
            psize = parseInt(s);
            if (psize == NaN || psize == null) psize = 0;
            return psize;
        },

        //===================================================

        page_reload: function () {
            api.init();
            var a = api.get_api_array_all_id();
            for (var k = 0; k < a.length; k++) {
                var id = a[k];
                var name = api.get_name(id);
                var v_id = api.get_cookie(name + '_id');
                var v_text = api.get_cookie(name + '_text');
                api.log(id + ' = ' + v_id + '=' + v_text);
                api.select_item(id, v_text, v_id);
            }
        },

        init: function (index, isPageLoaded) {
            _pageSize = api.get_page_size();
            //api.log('Get page size default: ' + _pageSize);

            var _all_id = '';
            var _es = document.getElementsByClassName('cob_iapiITCN');
            if (_es.length > 0) {
                //document.addEventListener('click', function (event) {
                //    api.popup_hide_all_after_click_out_api(event);
                //}, true);
            }
            for (var i = 0; i < _es.length; ++i)
            {
                if (index != null && index != i) continue;

                var cb = _es[i];
                //var id = this.get_xpath(cb) + '_' + i.toString();
                var id = 'api_' + api.guid().substring(0, 8) + '_' + i.toString();
                cb.setAttribute('id', id);
                //this.log(id);
                if (_all_id == '') _all_id = id; else _all_id += '|' + id;

                var page_path = api.get_page_path();
                var name = cb.getAttribute('name');
                if (name != null) {
                    api.set_cookie(page_path + '_' + name, id);
                    //api.set_cookie(name + '_id', '');
                    //api.set_cookie(name + '_text', '');
                }

                var item_default_id = cb.getAttribute('item_id');
                if (item_default_id == null) item_default_id = ''; else item_default_id = item_default_id.toString().trim();
                var item_default_text = cb.getAttribute('item_text');
                if (item_default_text == null) item_default_text = ''; else item_default_text = item_default_text.toString().trim();
                var _hasDefault = false;
                if (item_default_id != '' && item_default_text != '') _hasDefault = true;

                var it_footer_show_value = cb.getAttribute('footer');
                if (it_footer_show_value == null) it_footer_show_value = '1';

                var input_store = document.querySelectorAll('#' + id + ' .' + _css_input_items);
                for (var k = 0; k < input_store.length; ++k) {
                    input_store[k].innerHTML =
                        '<input type="text" placeholder="" value="' + item_default_text + '" id="' + id + _id_input + '" onclick="api.click_input(\'' + id + '\')" ondblclick="api.dblclick_input(\'' + id + '\')" onkeydown="api.keydown_input(event, \'' + id + '\', this.value)" onkeyup="api.keyup_input(event, \'' + id + '\', this.value)" />' +
                        '<input type="hidden" value="' + item_default_id + '" id="' + id + _id_item_selected_id + '"/>' +
                        '<input type="hidden" value="' + item_default_text + '" id="' + id + _id_item_selected_text + '"/>' +
                        '<input type="hidden" value="0" id="' + id + _id_total_items + '"/>' +
                        '<input type="hidden" value="' + (_hasDefault == true ? 'selected' : 'search') + '" id="' + id + _id_input_status + '"/>' +
                        '<input type="hidden" value="" id="' + id + _id_input_search + '"/>' +
                        '<input type="hidden" value="0" id="' + id + _id_input_parent_id + '"/>' +
                        '<input type="hidden" value="' + it_footer_show_value + '" id="' + id + _id_footer_show + '"/>' +
                        '<input type="hidden" value="0" id="' + id + _id_pagenumber + '"/>';
                    break;
                }

                var arrows = document.querySelectorAll('#' + id + ' .' + _css_button_arrow);
                for (var k = 0; k < arrows.length; ++k) {
                    arrows[k].setAttribute('id', id + _id_arrow);
                    arrows[k].setAttribute('onclick', 'api.click_arrow(\'' + id + '\')');
                }

                var pop = document.querySelectorAll('#' + id + ' .' + _css_popup_items);
                for (var k = 0; k < pop.length; ++k) {
                    var s_pop = '<div class="cob_iapiICH">                                                       ' +
                                    '    <div class="cobgx cob_iapiICHCL"></div>                                      ' +
                                    '    <div class="cobgx cob_iapiICHCM"></div>                                      ' +
                                    '    <div class="cobgx cob_iapiICHCR"></div>                                      ' +
                                    '</div>                                                                         ' +
                                    '<div class="cob_iapiICB">                                                       ' +
                                    '    <div class="cobgxlr cob_iapiICBL">                                           ' +
                                    '        <div class="cob_iapiICBLI"></div>                                       ' +
                                    '    </div>                                                                     ' +
                                    '    <div class="cobgx cob_iapiICBC" style="height:200px;min-height:200px;">      ' +
                                    '        <div class="cob_iapiLoad" id="' + id + _id_loading + '">Loading ...</div>' +
                                    '        <ul class="cobul">                                                      ' +
                                    '        </ul>                                                                  ' +
                                    '    </div>                                                                     ' +
                                    '    <div class="cob_iapiPage" id="' + id + _id_footer + '"></div>' +
                                    '    <div class="cobgxlr cob_iapiICBR">                                           ' +
                                    '        <div class="cob_iapiICBRI"></div>                                       ' +
                                    '    </div>                                                                     ' +
                                    '</div>                                                                         ' +
                                    '<div class="cob_iapiICF">                                                       ' +
                                    '    <div class="cobgx cob_iapiICFCL"></div>                                      ' +
                                    '    <div class="cobgx cob_iapiICFCM"></div>                                      ' +
                                    '    <div class="cobgx cob_iapiICFCR"></div>                                      ' +
                                    '</div>';

                    pop[k].innerHTML = s_pop;
                    pop[k].setAttribute('id', id + _id_popup_items);
                }

                var ls_item = document.querySelectorAll('#' + id + ' .' + _css_list_items);
                for (var k = 0; k < ls_item.length; ++k) {
                    ls_item[k].setAttribute('id', id + _id_list_items);
                    ls_item[k].setAttribute('onclick', 'api.click_item(event, \'' + id + '\')');
                    ls_item[k].setAttribute('onscroll', 'api.scroll_items(event, \'' + id + '\')');
                }
            }//end for
            api.set_cookie('api_all_id', _all_id);
        },

        footer_is_visibility: function (id) {
            var it = api.get_node(id + _id_footer_show);
            if (it.value == "1") return true;
            return false;
        },
        load_items_by_name: function (name) {
            var id = api.get_id(name);
            if (id != '' && id != null) {
                api.log('Bind items to -> ' + id);
                api.load_items(id);
                api.popup_hide(id);
            }
        },

        load_items_backgroud_by_name: function (name) {
            var id = api.get_id(name);

            var pageNumber = api.get_page_current(id);
            var total_ = api.get_total(id);
            if (total_ > 0 && total_ <= (pageNumber * _pageSize)) return;

            //api.popup_loading(id);

            var itext = api.get_node(id + _id_input);
            var text = '';
            if (itext != null) text = itext.value.toString().trim();
            //if (isTextSearch)
            //    api.set_input_search(id, text);
            //else
            if (api.get_input_status(id) == 'selected') {
                text = api.get_input_search(id);
                api.log('load_items -> is selected -> text = ' + text);
            }

            var name = api.get_name(id);
            if (name == null) name = '';
            var page_path = api.get_page_path();
            var parent_id = api.get_input_parent_id(id);
            var data = '_is_ajax=1&_api_page=' + page_path + '&_api_name=' + name + '&_parent_id=' + parent_id + '&_text=' + text + '&_pagesize=' + _pageSize + '&_pagenumber=' + pageNumber;
            this.log(data);

            var url = '/ajax.ashx';
            var it = api.get_node(id);
            if (it != null) {
                url = it.getAttribute('resource');
                if (url == null) url = location.href.toString();
            }
            api.ajax(data, url, {
                ok: function (val) {
                    api.log(val.substring(0, 55));

                    var _total = 0, _lenBlob = 0;
                    if (val != '') {
                        var a = val.split('#');
                        _lenBlob = a.length - 1;
                        if (_lenBlob == 0) {
                            api.set_total(id, 0);
                            var ifooter = api.get_node(id + _id_footer);
                            if (ifooter) ifooter.innerHTML = 'Displaying items 0-0 out of 0';

                            var it_items = api.get_node(id + _id_list_items);
                            if (it_items != null)
                                it_items.innerHTML = '<li><b>0</b><em>All</em></li>';

                            api.set_page_current(id, 0);

                            setTimeout(function () {
                                api.popup_show(id);
                            }, _timeOut_load_item_show);
                        }
                        else {
                            var a0 = a[0].trim().split('|');
                            _total = a0[0];
                            api.set_total(id, _total);

                            var ifooter = api.get_node(id + _id_footer);
                            var fshow = api.footer_is_visibility(id); api.log(fshow);
                            if (fshow && ifooter != null) {
                                ifooter.innerHTML = 'Displaying items 1-' + ((pageNumber * _pageSize) + _lenBlob) + ' out of ' + _total;
                                ifooter.style.display = 'block';
                            } else {
                                ifooter.style.display = 'none';
                            }

                            api.log(_lenBlob);

                            var item_sel_id = api.get_item_selected_id(id);
                            if (item_sel_id == null) item_sel_id = '';

                            var li = '';
                            if (pageNumber == 0) li = '<li><b>0</b><em>All</em></li>';
                            for (var k = 1; k < _lenBlob + 1; k++) {
                                var ai = a[k].split('|'), css_sel = '';
                                if (item_sel_id != '' && ai[0] == item_sel_id) {
                                    css_sel = ' class="sel" ';
                                    api.set_item_selected_text(id, ai[1]);
                                }
                                if (ai.length > 1) li += '<li' + css_sel + '><b>' + ai[0] + '</b><em>' + ai[1] + '</em></li>';
                            }

                            var it_items = api.get_node(id + _id_list_items);
                            if (it_items != null) {
                                if (pageNumber == 0)
                                    it_items.innerHTML = li;
                                else
                                    it_items.innerHTML += li;
                                api.set_page_current(id, pageNumber + 1);
                            }

                            //setTimeout(function () {
                            //    api.popup_show(id);
                            //}, _timeOut_load_item_show);
                        }
                    }
                },
                error: function () { }
            });
        },

        load_items: function (id, isTextSearch, isScroll) {
            if (isTextSearch == null) isTextSearch = false;
            if (isScroll == null) isScroll = false;

            var pageNumber = api.get_page_current(id);
            var total_ = api.get_total(id);
            if (total_ > 0 && total_ <= (pageNumber * _pageSize)) return;

            api.popup_loading(id);

            var itext = api.get_node(id + _id_input);
            var text = '';
            if (itext != null) text = itext.value.toString().trim();
            if (isTextSearch)
                api.set_input_search(id, text);
            else
                if (api.get_input_status(id) == 'selected') {
                    text = api.get_input_search(id);
                    api.log('load_items -> is selected -> text = ' + text);
                }

            var name = api.get_name(id);
            if (name == null) name = '';
            var page_path = api.get_page_path();
            var parent_id = api.get_input_parent_id(id);
            var data = '_is_ajax=1&_api_page=' + page_path + '&_api_name=' + name + '&_parent_id=' + parent_id + '&_text=' + text + '&_pagesize=' + _pageSize + '&_pagenumber=' + pageNumber;
            this.log(data);

            var url = '/ajax.ashx';
            var it = api.get_node(id);
            if (it != null) {
                url = it.getAttribute('resource');
                if (url == null) url = location.href.toString();
            }
            api.ajax(data, url, {
                ok: function (val) {
                    api.log(val.substring(0, 55));

                    var _total = 0, _lenBlob = 0;
                    if (val != '') {
                        var a = val.split('#');
                        _lenBlob = a.length - 1;
                        if (_lenBlob == 0) {
                            api.set_total(id, 0);
                            var ifooter = api.get_node(id + _id_footer);
                            if (ifooter) ifooter.innerHTML = 'Displaying items 0-0 out of 0';

                            var it_items = api.get_node(id + _id_list_items);
                            if (it_items != null)
                                it_items.innerHTML = '<li><b>0</b><em>All</em></li>';

                            api.set_page_current(id, 0);

                            setTimeout(function () {
                                api.popup_show(id);
                            }, _timeOut_load_item_show);
                        }
                        else {
                            var a0 = a[0].trim().split('|');
                            _total = a0[0];
                            api.set_total(id, _total);

                            var ifooter = api.get_node(id + _id_footer);
                            var fshow = api.footer_is_visibility(id); api.log(fshow);
                            if (fshow && ifooter != null) {
                                var _offset = (pageNumber + 1) * _pageSize;
                                if (_offset > _total) _offset = _total;

                                ifooter.innerHTML = 'Displaying items 1-' + _offset + ' out of ' + _total;
                                ifooter.style.display = 'block';
                            } else {
                                ifooter.style.display = 'none';
                            }

                            api.log(_lenBlob);

                            var item_sel_id = api.get_item_selected_id(id);
                            if (item_sel_id == null) item_sel_id = '';

                            var li = '';
                            if (pageNumber == 0) li = '<li><b>0</b><em>All</em></li>';
                            for (var k = 1; k < _lenBlob + 1; k++) {
                                var it0 = a[k];
                                var p0 = it0.indexOf('|');
                                if (p0 != -1) {
                                    var css_sel = '',
                                        id0 = it0.substring(0, p0),
                                        text0 = it0.substring(p0 + 1, it0.length);
                                    if (item_sel_id != '' && id0 == item_sel_id) {
                                        css_sel = ' class="sel" ';
                                        api.set_item_selected_text(id, text0);
                                    }
                                    li += '<li' + css_sel + '><b>' + id0 + '</b><em>' + text0 + '</em></li>';
                                }
                            }

                            var it_items = api.get_node(id + _id_list_items);
                            if (it_items != null) {
                                if (pageNumber == 0)
                                    it_items.innerHTML = li;
                                else
                                    it_items.innerHTML += li;
                                api.set_page_current(id, pageNumber + 1);
                            }

                            setTimeout(function () {
                                api.popup_show(id);
                            }, _timeOut_load_item_show);
                        }
                    }
                },
                error: function () { }
            });
        },

        set_input_parent_id_ny_name: function (name, parent_id) {
            var ele_id = api.get_id(name); api.log('set_input_parent_id_ny_name -> ' + name + ' = ' + ele_id + ' => ' + parent_id);
            api.set_input_parent_id(ele_id, parent_id);
        },
        set_input_parent_id: function (id, parent_id) {
            var ti = api.get_node(id + _id_input_parent_id);
            if (ti != null) ti.value = parent_id;
        },
        get_input_parent_id: function (id) {
            // search | selected
            var ti = api.get_node(id + _id_input_parent_id);
            if (ti != null)
                return ti.value;
            return null;
        },

        set_input_status: function (id, status) {
            var ti = api.get_node(id + _id_input_status);
            if (ti != null) ti.value = status;
        },
        get_input_status: function (id) {
            // search | selected
            var ti = api.get_node(id + _id_input_status);
            if (ti != null)
                return ti.value;
            return null;
        },

        set_input_search: function (id, text) {
            var ti = api.get_node(id + _id_input_search);
            if (ti != null) ti.value = text;
        },
        get_input_search: function (id) {
            // search | selected
            var ti = api.get_node(id + _id_input_search);
            if (ti != null)
                return ti.value;
            return null;
        },

        get_item_click_id: function (event) {
            var it = event.target;
            if (it.nodeName == 'B')
                return it.innerHTML;
            return null;
        },
        get_item_click_text: function (event) {
            var it = event.target;
            if (it.nodeName == 'B') {
                var s = it.parentNode.innerHTML;
                var pos = s.indexOf('<em>');
                if (pos > 0) {
                    var si = s.substring(pos + 4, s.length - 5);
                    return si;
                }
            }
            return null;
        },

        get_item_selected_id: function (id) {
            var it = api.get_node(id + _id_item_selected_id);
            if (it != null)
                return it.value;
            return null;
        },
        get_item_selected_text: function (id) {
            var it = api.get_node(id + _id_item_selected_text);
            if (it != null)
                return it.value;
            return null;
        },
        set_item_selected_id: function (id, value) {
            var it = api.get_node(id + _id_item_selected_id);
            if (it != null)
                it.value = value;
        },
        set_item_selected_text: function (id, text) {
            var it = api.get_node(id + _id_item_selected_text);
            if (it != null)
                it.value = text;
        },

        get_total: function (id) {
            var it = api.get_node(id + _id_total_items);
            var k = 0;
            if (it != null) k = parseInt(it.value);
            return k;
        },
        set_total: function (id, total) {
            var it = api.get_node(id + _id_total_items);
            if (it != null) it.value = total;
        },
        get_page_current: function (id) {
            var it = api.get_node(id + _id_pagenumber);
            var k = 0;
            if (it != null) k = parseInt(it.value);
            return k;
        },
        set_page_current: function (id, page) {
            var it = api.get_node(id + _id_pagenumber);
            if (it != null) it.value = page;
        },

        scroll_items: function (e, id) {
            var lsv = api.get_node(id + _id_list_items);
            if (lsv.offsetHeight + lsv.scrollTop >= lsv.scrollHeight) {
                this.log('is bottom --> ajax call -> ....');
                clearTimeout(_typingTimer);
                setTimeout(function () {
                    api.load_items(id, false);
                }, _timeOut_scroll_call_ajax);
            }
        },
        keyup_input: function (e, id, text) {
            clearTimeout(_typingTimer);
            _typingTimer = setTimeout(function () {
                api.log('typing: -> ' + text);
                api.set_page_current(id, 0);
                api.load_items(id, true);
            }, _doneTypingInterval);
        },
        keydown_input: function (e, id, text) {
            clearTimeout(_typingTimer);
        },

        dblclick_input: function (id) {
            api.select_clear(id);
            api.set_input_search(id, '');
            clearTimeout(_typingTimer);
            api.set_page_current(id, 0);
            api.load_items(id, false);
        },
        click_input: function (id) {
            api.popup_hide_all_except_id(id);
            api.popup_toggle(id);
        },

        click_arrow: function (id) {
            api.popup_hide_all_except_id(id);
            api.popup_toggle(id);
        },

        click_item: function (ev, id) {
            ////api_id, api_name, item_id, item_text

            var it_id = api.get_item_click_id(ev);
            var it_text = api.get_item_click_text(ev);
            if (it_id != null && it_text != null) {
                this.log('click -> ' + it_id + '=' + it_text);

                var it = ev.target.parentNode;
                var lis = it.parentNode.querySelectorAll('li');
                for (var k = 0; k < lis.length; ++k)
                    lis[k].className = '';
                if (it != null) it.className = 'sel';

                api.select_item(id, it_text, it_id);
                api.popup_hide(id);

                var page_path = api.get_page_path();
                var funName = page_path + '_OnSelectedIndexChanged';

                api.log(funName + '| click -> ' + it_id + '=' + it_text);
                var fn = window[funName];
                if (typeof fn === "function") {
                    var name = api.get_name(id);
                    fn(id, name, it_id, it_text);
                }
            }
        },

        select_item: function (id, item_text, item_id) {
            var itn = api.get_node(id + _id_input);
            if (itn != null) itn.value = item_text;

            api.set_item_selected_id(id, item_id);
            api.set_item_selected_text(id, item_text);

            var name = api.get_name(id);
            if (name != null) {
                api.set_cookie(name + '_id', item_id);
                api.set_cookie(name + '_text', item_text);
            }
            api.set_input_status(id, 'selected');
        },

        select_clear: function (id) {
            var it_text = api.get_node(id + _id_input);
            if (it_text != null) it_text.value = '';

            api.set_item_selected_id(id, '');
            api.set_item_selected_text(id, '');
        },

        clear_all_by_name: function (name) {
            clearTimeout(_typingTimer);

            var id = api.get_id(name);
            var a = id.split('_');
            var index = parseInt(a[a.length - 1]);
            api.init(index);

            //var it_text = api.get_node(id + _id_input);
            //if (it_text != null) it_text.value = '';

            //var its = api.get_node(id + _id_list_items);
            //if (its != null) {
            //    its.scrollTo(0, 0);
            //    //its.scrollTop = 0;
            //    its.innerHTML = '';
            //}

            //api.set_input_search(id, '');

            //api.set_cookie(name + '_id', '');
            //api.set_cookie(name + '_text', '');

            //api.set_item_selected_id(id, '0');
            //api.set_item_selected_text(id, '');

            //api.set_total(id, 0);
            //api.set_page_current(id, 0);

            //api.set_input_status(id, 'search');
            //api.set_input_parent_id(id, '0');
        },

        //===================================================

        popup_toggle: function (id) {
            var pop_items = api.get_node(id + _id_popup_items);
            if (pop_items != null) {
                if (pop_items.style.display == 'none') {
                    var total = api.get_total(id);
                    if (total == 0) {
                        clearTimeout(_typingTimer);
                        api.set_page_current(id, 0);
                        api.load_items(id, false);
                    }
                    else {
                        api.popup_show(id);
                    }
                }
                else {
                    api.popup_hide(id);
                }
            }
        },
        popup_show: function (id) {
            var pop_items = api.get_node(id + _id_popup_items);
            if (pop_items != null)
                pop_items.style.display = 'block';

            var it_load = api.get_node(id + _id_loading);
            if (it_load != null)
                it_load.style.display = 'none';

            var it_items = api.get_node(id + _id_list_items);
            if (it_items != null)
                it_items.style.display = 'block';

            var it_footer = api.get_node(id + _id_footer);
            if (it_footer != null)
                it_footer.style.display = 'block';
        },
        popup_hide: function (id) {
            var pop_items = api.get_node(id + _id_popup_items);
            if (pop_items != null)
                pop_items.style.display = 'none';
        },
        popup_hide_all_except_id: function (id) {
            var a = api.get_api_array_all_id();
            if (a.length > 0) {
                for (var i = 0; i < a.length; i++) {
                    var _id = a[i].trim();
                    if (_id != id) {
                        api.log('HIDE  -> ' + _id);
                        api.popup_hide(_id);
                    }
                }
            }
        },
        popup_hide_all: function () {
            var a = api.get_api_array_all_id();
            api.log(a);
            if (a.length > 0) {
                for (var i = 0; i < a.length; i++) {
                    api.popup_hide(a[i]);
                }
            }
        },
        popup_hide_all_after_click_out_api: function (event_click) {
            var it = event_click.target;
            var _self = api.get_closest(it, '.cobs');
            api.log('document -> click ...');
            api.log(it);
            api.log(_self);
            if (_self == null) // can not find kit dropdownlist ....
            {
                api.log('event click: -----> OUT combo --> HIDE POPUP')
                setTimeout(function () {
                    api.popup_hide_all();
                }, 100);
                event_click.preventDefault();
            } else {
                var _id = _self.getAttribute('id');
                if (_id != null) {
                    setTimeout(function () {
                        api.popup_hide_all_except_id(_id);
                    }, 100);
                }
                api.log('event click: -----> ON combo ---> break ... -> HIDE except: ' + _id);
            }
        },
        popup_loading: function (id) {
            var pop_items = api.get_node(id + _id_popup_items);
            if (pop_items != null) {
                pop_items.style.display = 'block';

                var it_items = api.get_node(id + _id_list_items);
                if (it_items != null)
                    it_items.style.display = 'none';

                var it_footer = api.get_node(id + _id_footer);
                if (it_footer != null)
                    it_footer.style.display = 'none';

                var it_load = api.get_node(id + _id_loading);
                if (it_load != null)
                    it_load.style.display = 'block';
            }
        },

        //===================================================
        msg_id: function (mid) {
            var d = api.get_number_id(1, 9);
            var s = mid + d;
            return s;
        },
        //===================================================
        view_get_result: function (config, value) {

        },
        view_get_process: function (config, value) {

        },
        view_get: function (view_name) {
            //var id = api.msg_id(msLAYER.API);
            //var f = {};
            //f['name'] = 'VIEW_GET';
            //f['data'] = view_name;
            //f['msg_id'] = id;
            //f['viewid'] = api.get_cookie('viewid');
            //f['callback'] = 'api.view_get_result';
            //f['callback_process'] = 'api.view_get_process';
            //f['msg_id'] = id;
            //f['sessionid'] = api.get_cookie('sessionid');
            //var json = JSON.stringify(f);

            //sessionStorage[id + '.api'] = json;

            //var b64 = 'para=' + id + window.btoa(json); 
            ////var decodedData = window.atob(encodedData);
            //api.ajax(b64, '/api/view', null);
        },
        //===================================================
        //===================================================

        admin: function () {
            api.popup_login();

            //api.view_get('adm.layout');
        },
        admin_layout__setContent: function (layout_code, content) {
            w2ui['main_layout'].content(layout_code, content);
        },
        admin_grid__reload: function (grid_key) {
            w2ui[grid_key].reload();
        },
        admin_grid_toolbar__click: function (target, eventData) {
            var a = target.toString().split(':');
            if (a.length == 1) {
                var o = eventData.item;
                if (o != null && o['_model'] != null) {
                    o['_tab'] = sessionStorage['tab'];
                    o['_viewid'] = api.get_cookie('viewid');
                    o['_sessionid'] = api.get_cookie('sessionid');
                    o['_username'] = sessionStorage['user.username'];

                    var json = JSON.stringify(o);
                    var b64 = decodeURIComponent(json);

                    api.ajax(b64, '/api/link', {
                        ok: function (val) {
                            textArea_htmlDecode.innerHTML = val;
                            var data = textArea_htmlDecode.value;
                            //console.log(data);
                            eval(data);
                        }
                    });

                    //console.log(o);
                }
            } else {
                var id = parseInt(a[1]);
                if (id > 0) {
                    var it = eventData.item.items;
                    var o = it[id - 1];
                    if (o != null && o['_model'] != null) {
                        o['_tab'] = sessionStorage['tab'];
                        o['_viewid'] = api.get_cookie('viewid');
                        o['_sessionid'] = api.get_cookie('sessionid');
                        o['_username'] = sessionStorage['user.username'];

                        o['id'] = o['tag'];
                        o['tag'] = '';

                        var json = JSON.stringify(o);
                        var b64 = decodeURIComponent(json);

                        api.ajax(b64, '/api/action', {
                            ok: function (val) {
                                textArea_htmlDecode.innerHTML = val;
                                var data = textArea_htmlDecode.value;
                                //console.log(data);
                                eval(data);
                            }
                        });

                        //console.log(o);
                    }
                }
            }
        },
        admin_tab_change: function (event) {
            var tab_code = event.target.toString();
            tab_code = tab_code.substring(4, tab_code.length);
            sessionStorage['tab'] = tab_code;

            var action_ = 'api_config_tab_' + tab_code;
            var it = document.getElementById(action_);
            if (it == null) {
                api.api_lock();
                var fd = {
                    'lock': 'true',
                    'control_redirect': '',
                    'action_redirect': ''
                };
                api.api_post(msLAYER.API_CONFIG, 'tab', tab_code, fd, { ok: function (val) { } });
            } else {
                var fnName = 'tab_' + tab_code + '_reload';
                if (eval('typeof ' + fnName) === "function")
                    window[fnName](null);
                else {
                    api.admin_layout__setContent('main', 'Can not find function: ' + fnName);
                }
            }
        },
        admin_tab_grid_search: function (event, tabgrid_name, tab_code, text) {
            api.log(event);
            api.log(text);
            if (event.keyCode === 13 || event.which == 13) {
                var name = 'search_' + tabgrid_name;
                api.set_cookie(name, text.trim());
                w2ui[tabgrid_name].reload();
                setTimeout(function () {
                    var it = document.getElementById(name);
                    if (it != null) it.value = text;
                }, 1000);
            }
        },
        admin_tab_grid_search_text_set: function (tabgrid_name) {
            setTimeout(function () {
                var name = 'search_' + tabgrid_name;
                var text = api.get_cookie(name);
                var it = document.getElementById(name);
                if (it != null)
                    it.value = text;
            }, 1000);
        },
        //===================================================
        api_post: function (msLAYER_ID, control, action, obj, event) {
            var mid = api.msg_id(msLAYER_ID);
            //var mid = api.msg_id(msLAYER.API);
            var sid = api.get_cookie('sessionid');
            var vid = api.get_cookie('viewid');
            var f = {};
            f['control'] = control;
            f['action'] = action;
            f['msg_id'] = mid;
            f['viewid'] = vid;
            f['callback'] = 'api_post_result';
            f['callback_process'] = 'api_post_process';
            f['sessionid'] = sid;

            var data = JSON.stringify(obj);
            sessionStorage[mid + '.data'] = data;
            f['data'] = window.btoa(data);

            f['control_redirect'] = obj['control_redirect'] == null ? '' : obj['control_redirect'];
            f['action_redirect'] = obj['action_redirect'] == null ? '' : obj['action_redirect'];
            obj['control_redirect'] = '';
            obj['action_redirect'] = '';

            var json = JSON.stringify(f);
            var b64 = 'msg=' + vid + ',' + sid + ',' + mid + ',' + control + ',' + action + ',' + window.btoa(json);
            //var decodedData = window.atob(encodedData); 
            sessionStorage[mid + '.msg'] = json;
            sessionStorage[mid + '.lock'] = obj['lock'] == null ? 'false' : obj['lock'];

            api.log(' api_post: --> ');
            api.log(f);

            api.ajax(b64, '/api', event);
        },
        api_lock: function () {
            var load = document.getElementById('loading___');
            if (load != null) {
                load.style.display = 'inline-block';
            }
        },
        api_unlock: function () {
            var load = document.getElementById('loading___');
            if (load != null) {
                load.style.display = 'none';
            }
        },
        api_post_result: function (mid, value) {
        },
        api_post_process: function (mid, value) {

        },
        api_msg_redirect: function (f) {
            var vid = api.get_cookie('viewid');
            var sid = api.get_cookie('sessionid');
            var mid = f['msg_id'];

            var data = JSON.stringify(f['data']);
            sessionStorage[mid + '.data'] = data;
            f['data'] = window.btoa(f['data']);

            var json = JSON.stringify(f);
            var b64 = 'msg=' + vid + ',' + sid + ',' + mid + ',' + f['control'] + ',' + f['action'] + ',' + window.btoa(json);
            //var decodedData = window.atob(encodedData); 
            sessionStorage[mid + '.msg'] = json;
            sessionStorage[mid + '.lock'] = f['lock'] == null ? 'false' : f['lock'];

            api.log(' api_msg_redirect: --> ');
            api.log(f);

            api.ajax(b64, '/api', null);
        },
        api_redirect: function (objectApi, msLAYER_ID) {
            var o = objectApi;
            //o['control'] = re_control;
            //o['action'] = re_action;

            o['control_redirect'] = '';
            o['action_redirect'] = '';

            var mid = o['msg_id'];
            if (mid != null) {
                if (msLAYER_ID == null)
                    mid = mid.substring(0, 21) + api.get_number_min_max(1000, 9999);
                else {
                    api.log('---> ' + mid);
                    mid = msLAYER_ID.toString() + mid.substring(7, 21) + api.get_number_min_max(1000, 9999);
                    api.log('<--- ' + mid);
                }
            }
            o['msg_id'] = mid;

            api.api_msg_redirect(o);
        },
        //===================================================
        form: function (url, params) {
            //var params = { key: 'keyword' };
            //var form = $('<form method="POST" action="/file-temp">');
            //var form = document.createElement('form');
            _apiform.setAttribute('method', 'POST');
            _apiform.setAttribute('action', url);
            var s = '';
            $.each(params, function (k, v) {
                s += '<input type="hidden" name="' + k + '" value="' + v + '">';
            });
            _apiform.innerHTML = s;
            _apiform.submit();
        },
        alert: function (msg) {
            w2alert(msg, 'Thông báo');
        },
        decodeHTMLEntities: function (text) {
            var entities = [
                ['amp', '&'],
                ['apos', '\''],
                ['#x27', '\''],
                ['#x2F', '/'],
                ['#39', '\''],
                ['#47', '/'],
                ['lt', '<'],
                ['gt', '>'],
                ['nbsp', ' '],
                ['quot', '"']
            ];

            for (var i = 0, max = entities.length; i < max; ++i)
                text = text.replace(new RegExp('&' + entities[i][0] + ';', 'g'), entities[i][1]);

            return text;
        },
        decode_b64_html: function (text) {
            text = window.atob(text);

            textArea_htmlDecode.innerHTML = text;
            text = textArea_htmlDecode.value;

            return text;
        },
        //===================================================
    };

    return api;
}));
var _apiform = document.createElement('form');




//api.login_check();

function f_api_init() {
    api.init();
    //api.popup_login();
}

f_api_init();

// INIT combobox
//<body onload="f_init_combo()">
//function f_init_combo() {
//    var fn = window['f_api_init'];
//    if (typeof fn === "function") fn();
//}

 












//#region [ FUNCTION ]

//console.log(browser_width + ',' + browser_height)


var pop_demo_options = {
    height: 300,
    title: 'Tiêu đề',
    bg: false,
    close: function (pid) { },
    form: {
        name: api.get_number_id(),
        overflow: 'hidden',
        url: 'server/post',
        fields: [
            { field: 'first_name', type: 'text', required: true, html: { caption: 'First Name', attr: 'style="width: 200px"' } },
            { field: 'last_name', type: 'text', required: true, html: { caption: 'Last Name', attr: 'style="width: 200px"' } },
            { field: 'comments', type: 'textarea', html: { caption: 'Comments', attr: 'style="width: 200px; height: 190px"' } }
        ],
        actions: {
            'Save': function (event) {
                console.log('save', event);
                this.save();
            },
            'Clear': function (event) {
                console.log('clear', event);
                this.clear();
            },
        }
    }
};

//popup(pop_demo_options);


//#endregion