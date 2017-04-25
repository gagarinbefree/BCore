/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/highlightjs/highlightjs.d.ts" />
/// <reference path="../../typings/ace/index.d.ts" />

module Blog {
    export class Code {
        private owner: WhatsNew;
        private form: JQuery = $('#codeModalForm');
        private code: JQuery = $('#codeText');

        constructor(owner: WhatsNew) {
            this.owner = owner;

            this.init();
        }

        private init(): void {
            this.form.on('shown.bs.modal', (e: JQueryEventObject) => this.formShow(e));
            //this.text.on('input change keyup cut', (e: JQueryEventObject) => this.highlight(e));
        }

        private formShow(e: JQueryEventObject): void {            
            var editor = ace.edit('codeText');
            this.code.focus();
        }

        //private highlight(e: JQueryEventObject) {
        //    hljs.highlightBlock(e.target);
        //}
    }
}