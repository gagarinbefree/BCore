/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/highlightjs/highlightjs.d.ts" />
/// <reference path="../../typings/ace/index.d.ts" />

module Blog {
    export class Code {
        private owner: WhatsNew;
        private form: JQuery = $('#codeModalForm');
        private editor: AceAjax.Editor;

        constructor(owner: WhatsNew) {
            this.owner = owner;

            this.init();
        }

        private init(): void {
            this.form.on('shown.bs.modal', (e: JQueryEventObject) => this.formShow(e));            
        }

        private formShow(e: JQueryEventObject): void {
            this.initAceEditor();
        }

        private initAceEditor(): void {
            this.editor = ace.edit('codeText');
            this.editor.setTheme("ace/theme/twilight");
            this.editor.renderer.setShowGutter(false);
            this.editor.focus();
        }
    }
}