/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />

module Post {
    export class Comment {

        private elBody: JQuery = $("body");
        private elInput: JQuery = $("#commentInput");
        private elDropdown: JQuery = $(".dropdown-toggle");

        constructor() {
            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);

            this.elDropdown.dropdown();
        }
    }
}