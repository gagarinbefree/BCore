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
            var _this = this;
            e.preventDefault();
            this.elTarget = $(e.target);
            bootbox.confirm({
                title: this.elTarget.attr('data-confirm-title'),
                message: this.elTarget.attr('data-confirm'),
                size: 'small',
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
                callback: function (result) { return _this.dialogResult(result); }
            });
        };
        Confirm.prototype.dialogResult = function (result) {
            if (result) {
                var href = this.elTarget.attr('href');
                if (typeof (href) !== "undefined" && href && href.length > 0)
                    window.location.href = href;
            }
        };
        return Confirm;
    }());
    Common.Confirm = Confirm;
})(Common || (Common = {}));
