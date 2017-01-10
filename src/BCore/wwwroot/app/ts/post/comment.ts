/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
/// <reference path="../../typings/jquery.appear/jquery.appear.js.d.ts" />

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

            this.elInput.appear();
            this.elInput.on("appear", (e: JQueryEventObject, elements: JQuery) => this.inputAppear(e, elements));
        }

        private inputAppear(e: JQueryEventObject, elements: JQuery): void {
            elements.focus();
        }
    }
}