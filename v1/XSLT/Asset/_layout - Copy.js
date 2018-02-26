

var pstyle = 'border: 1px solid #ccc; padding:0px;';
var main_config =
    {
        main_layout: {
            name: 'main_layout',
            panels: [
                {
                    type: 'top', size: 40, resizable: false, style: '',
                    toolbar: {
                        //name: 'main_toolbar',
                        items: [
                            {
                                type: 'menu', caption: '<div id="user_name"></div>', icon: 'fa-user', items: [
                                    { type: 'button', id: 'b_changepass', caption: 'Change password', icon: 'fa-key' },
                                ]
                            },
                            { type: 'spacer' },
                            { type: 'menu', caption: 'System', icon: 'fa-cog', items: [{}] },
                            { type: 'button', id: 'b_logout', caption: 'LogOut', icon: 'fa-signout' },
                            { type: 'button', id: 'b_help', caption: '', icon: 'fa-question-sign' }
                        ],
                        onRender: function (event) {
                            event.onComplete = function () {
                            }
                        },
                        onClick: function (event) {
                            console.log('Target: ' + event.target, event);
                            switch (event.target) {
                                case 'b_logout':
                                    api.logout();
                                    break;
                                case 'b_help':
                                    api.alert('Sample');
                                    break;
                            }
                        }
                    }
                },
                {
                    type: 'left', size: 236, resizable: true, style: pstyle, hidden: true
                },
                {
                    type: 'main',
                    style: pstyle + 'border-top: 0px;',
                    tabs: {
                        active: 'tab_email',
                        tabs: [{}],
                        onClick: function (event) {
                            api.admin_tab_change(event);
                        }
                    }
                },
                {
                    type: 'right', size: 350, resizable: true, style: pstyle, hidden: true
                },
                { type: 'preview', size: '100%', resizable: false, hidden: true, style: pstyle, title: 'Table Explorer' },
                { type: 'bottom', size: '50%', resizable: true, hidden: true, style: pstyle }
            ],
            onResize: function (event) {
                event.onComplete = function () {
                    f_main_Resize();
                }
            },
            onRender: function (event) {
                event.onComplete = function () {
                    setTimeout(function () {
                        f_main_Ready();
                        w2ui['main_layout_top_toolbar'].click('b_db');
                    }, 100);
                }
            }
        }
    };

function f_main_Ready() {
    //w2ui['main_layout_main_tabs'].click('tabmain');
}

function f_main_Resize() {
}

$(document).ready(function () {
    $('#main_layout').w2layout(main_config.main_layout);
});


