/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/highlightjs/highlightjs.d.ts" />
/// <reference path="../../typings/ace/index.d.ts" />
var Blog;
(function (Blog) {
    var Code = (function () {
        function Code(owner) {
            this.form = $('#codeModalForm');
            this.code = $('#codeText');
            this.owner = owner;
            this.init();
        }
        Code.prototype.init = function () {
            var _this = this;
            this.form.on('shown.bs.modal', function (e) { return _this.formShow(e); });
            //this.text.on('input change keyup cut', (e: JQueryEventObject) => this.highlight(e));
        };
        Code.prototype.formShow = function (e) {
            var editor = ace.edit('codeText');
            this.code.focus();
        };
        return Code;
    }());
    Blog.Code = Code;
})(Blog || (Blog = {}));
//# sourceMappingURL=code.js.map