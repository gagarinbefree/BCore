/// <reference path="../typings/bootbox/index.d.ts" />

module Common {
    export class App {
        private confirm: ConfirmDialog;        

        constructor() {
            this.confirm = new ConfirmDialog();
        }
    }

    export class ConfirmDialog {
        private elConfirm: JQuery = $('a[data-confirm]');        
        private elTarget: JQuery;

        constructor() {
            this.elConfirm.on('click', (e: JQueryEventObject) => this.confirm(e));
        }

        private confirm(e: JQueryEventObject): void {
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

        private handlerDialogResult(result: boolean) {
            if (result)
            {
                var href: string = this.elTarget.attr('href');
                if (typeof (href) !== "undefined" && href && href.length > 0)
                    window.location.href = href;
            }
        }
    }
}