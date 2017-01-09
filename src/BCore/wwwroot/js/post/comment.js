/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
var Post;
(function (Post) {
    var Comment = (function () {
        function Comment() {
            this.elBody = $("body");
            this.elInput = $("#commentInput");
            this.elDropdown = $(".dropdown-toggle");
            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elDropdown.dropdown();
        }
        return Comment;
    }());
    Post.Comment = Comment;
})(Post || (Post = {}));
