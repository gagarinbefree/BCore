/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
var Blog;
(function (Blog) {
    var WhatsNew = (function () {
        function WhatsNew(userId) {
            var _this = this;
            this.elSubmitForm = $("#whatsNewForm");
            this.elHiddenUrl = $("#whatsNewImageUrl");
            this.elInput = $("#whatsNewInput");
            this.elPostButton = $("#whatsNewPostButton");
            this.elPost = $("#whatsNewPost");
            this.elDropdown = $(".dropdown-toggle");
            this.userId = userId;
            this.elPostButton.on("click", function (e) { return _this.post(e); });
            this.init();
        }
        WhatsNew.prototype.init = function () {
            var image = new Blog.Image(this);
            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elInput.focus();
            this.elHiddenUrl.val("");
            this.elDropdown.dropdown();
        };
        WhatsNew.prototype.post = function (e) {
            var _this = this;
            e.preventDefault();
            this.elPost.load("/Blog/Post", this.elSubmitForm.serializeArray(), function () { return _this.init(); });
        };
        return WhatsNew;
    }());
    Blog.WhatsNew = WhatsNew;
})(Blog || (Blog = {}));
