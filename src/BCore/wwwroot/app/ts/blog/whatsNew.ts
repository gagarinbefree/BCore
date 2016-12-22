/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/autosize/autosize.d.ts" />
/// <reference path="../../typings/respify/respify.d.ts" />

module Blog {
    export class WhatsNew {
        public userId: string;
        public elSubmitForm: JQuery;
        public elHiddenUrl: JQuery;        

        private elInput: JQuery;
        private elPostButton: JQuery;
        private elPost: JQuery;
        
        private el16x9: JQuery;
     
        constructor(userId: string) {
            this.userId = userId;

            this.elSubmitForm = $("#whatsNewForm");
            this.elInput = $("#whatsNewInput");
            this.elPostButton = $("#whatsNewPostButton");
            this.elPost = $("#whatsNewPost");
            this.elHiddenUrl = $("#whatsNewImageUrl");                                    

            this.elPostButton.on("click", (e) => this.post(e));                     
           
            //this.el16x9 = $(".img-responsive2");


            debugger;

            //$('#default-responsive').respify();

            

            //this.el16x9.respify({});

            this.init();
        }

        private init(): void {
            var image = new Image(this);

            autosize(this.elInput);
            this.elInput.val("");
            autosize.update(this.elInput);
            this.elInput.focus();
            this.elHiddenUrl.val("");                        
        }

        public post(e: Event): void {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: "/Blog/Post",
                data: this.elSubmitForm.serializeArray(),
                success: (htmlString: string) => this.htmLoadSuccess(htmlString)                
            });


            //this.elPost.load("/Blog/Post"
            //    , this.elSubmitForm.serializeArray()
            //    , () => this.init());
        }

        private htmLoadSuccess(htmlString: string) {
            this.elPost.html(htmlString);

            debugger;

            //$(".post-image-container").imagefill({});            

            this.init();
        }             
    }
}