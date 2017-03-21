/// <reference path="../typings/bootbox/index.d.ts" />


module Common {
    export class App {
        private confirm: Confirm;

        constructor() {
            this.confirm = new Confirm();
        }
    }

    export class Confirm {
        private elConfirm: JQuery = $('a[data-confirm]');

        constructor() {
            this.elConfirm.on('click', (e: JQueryEventObject) => this.confirm(e));
        }

        private confirm(e: JQueryEventObject): void {
            e.preventDefault();

            bootbox.confirm({
                message: $(e.target).attr('data-confirm'),                
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

                callback: function (result) {
                    if (result) {
                    }
                }
            });
        }
    }
}