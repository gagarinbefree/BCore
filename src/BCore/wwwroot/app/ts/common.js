/// <reference path="../typings/bootbox/index.d.ts" />
var Common;
(function (Common) {
    var App = (function () {
        function App() {
            this.confirm = new Confirm();
        }
        return App;
    }());
    Common.App = App;
    var Confirm = (function () {
        function Confirm() {
            var _this = this;
            this.elConfirm = $('a[data-confirm]');
            this.elConfirm.on('click', function (e) { return _this.confirm(e); });
        }
        Confirm.prototype.confirm = function (e) {
            e.preventDefault();
            bootbox.confirm({
                message: $(e.target).attr('data-confirm'),
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-success'
                    }
                },
                callback: function (result) {
                    if (result) {
                    }
                }
            });
        };
        return Confirm;
    }());
    Common.Confirm = Confirm;
})(Common || (Common = {}));
//# sourceMappingURL=common.js.map