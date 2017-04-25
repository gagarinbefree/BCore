﻿/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />

module Blog {
    export class WhatsNew {
        public userId: string;
        public elSubmitForm: JQuery = $("#whatsNewForm");

        private elInput: JQuery = $("#whatsNewText");
        public elImageUrl = $("#whatsNewImageUrl");
        public elVideoUrl = $("#whatsNewVideoUrl");
        public elGeo = $("#whatsNewGeo");
        
        private elPostButton: JQuery = $("#whatsNewPostButton");
        private elPost: JQuery = $("#whatsNewPost");

        private elDropdown = $(".dropdown-toggle"); 

        private modalImage: Image;
        private modalCode: Code;
     
        constructor(userId: string) {
            this.userId = userId;            

            this.elPostButton.on("click", (e) => this.post(e));                     

            this.init();
        }

        private init(): void {
            this.modalImage = new Image(this);
            this.modalCode = new Code(this);

            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elInput.focus();
            this.elImageUrl.val("");
            this.elDropdown.dropdown();                  
        }

        public post(e: Event): void {
            e.preventDefault();

            this.elPost.load("/Update/Post"
                , this.elSubmitForm.serializeArray()
                , () => this.init());
        }
    }
}