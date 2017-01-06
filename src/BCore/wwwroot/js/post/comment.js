/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
var Post;
(function (Post) {
    var Comment = (function () {
        function Comment() {
            this.elBody = $("body");
            this.elInput = $("#commentInput");
            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            //if (location.hash == "#comments")
            //    this.elInput.focus();
        }
        return Comment;
    }());
    Post.Comment = Comment;
})(Post || (Post = {}));
