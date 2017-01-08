/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />

module Blog {
    export class WhatsNew {
        public userId: string;
        public elSubmitForm: JQuery = $("#whatsNewForm");
        public elHiddenUrl = $("#whatsNewImageUrl");

        private elInput: JQuery = $("#whatsNewInput");
        private elPostButton: JQuery = $("#whatsNewPostButton");
        private elPost: JQuery = $("#whatsNewPost");

        private elDropdown = $(".dropdown-toggle"); 
     
        constructor(userId: string) {
            this.userId = userId;            

            this.elPostButton.on("click", (e) => this.post(e));                     

            this.init();
        }

        private init(): void {
            var image = new Image(this);

            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elInput.focus();
            this.elHiddenUrl.val("");
            this.elDropdown.dropdown();                  
        }

        public post(e: Event): void {
            e.preventDefault();

            this.elPost.load("/Blog/Post"
                , this.elSubmitForm.serializeArray()
                , () => this.init());
        }
    }
}