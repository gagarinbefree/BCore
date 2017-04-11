/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/bootbox/index.d.ts" />

module Common {
    export class App {
        private confirm: ConfirmDialog;        

        constructor() {
            this.confirm = new ConfirmDialog();            
        }
    }

    export class Pager {
        private url: string;
        private page: number;
        private elContainer: JQuery;
        private elButtonDone: JQuery;

        constructor(url, page, elContainer, elButtonDone) {
            this.url = url;
            this.page = Number(page);
            this.elContainer = elContainer;
            this.elButtonDone = elButtonDone;
            if (this.elButtonDone)
                this.elButtonDone.on('click', (e: JQueryEventObject) => this.loadPage(e));                 
        }

        private loadPage(e: JQueryEventObject): void {

            debugger;

            if (this.elContainer)
                this.elContainer.load(this.url, { page: this.page + 1 }, (responseText: string, textStatus: string, XMLHttpRequest: XMLHttpRequest) => this.init(responseText, textStatus, XMLHttpRequest));                
        }

        private init(responseText: string, textStatus: string, XMLHttpRequest: XMLHttpRequest) {
            debugger;
        }
    }

    export class ConfirmDialog {
        private elConfirm: JQuery = $('a[data-confirm]');        
        private elTarget: JQuery;

        constructor() {
            this.elConfirm.on('click', (e: JQueryEventObject) => this.showDialog(e));
        }

        private showDialog(e: JQueryEventObject): void {
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
                callback: (result: boolean) => this.handlerDialogResult(result)
            });
        }

        private handlerDialogResult(result: boolean) : void {
            if (result) {
                var href: string = this.elTarget.attr('href');
                if (typeof (href) !== "undefined" && href && href.length > 0)
                    window.location.href = href;
            }
        }
    }
}