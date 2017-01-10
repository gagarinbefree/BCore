/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
/// <reference path="../../typings/jquery.appear/jquery.appear.js.d.ts" />
var Post;
(function (Post) {
    var Comment = (function () {
        function Comment() {
            var _this = this;
            this.elBody = $("body");
            this.elInput = $("#commentInput");
            this.elDropdown = $(".dropdown-toggle");
            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elDropdown.dropdown();
            this.elInput.appear();
            this.elInput.on("appear", function (e, elements) { return _this.inputAppear(e, elements); });
        }
        Comment.prototype.inputAppear = function (e, elements) {
            elements.focus();
        };
        return Comment;
    }());
    Post.Comment = Comment;
})(Post || (Post = {}));
