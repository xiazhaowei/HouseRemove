var Script = function () {
    jQuery.extend(jQuery.validator.messages, {
        required: "请填写此项。",
        remote: "请修正此项。",
        email: "请输入一个合法的电子邮件地址。",
        url: "请输入合法网址。",
        date: "请输入合法日期。",
        dateISO: "请输入合法日期（ISO格式）。",
        number: "请输入合法数字。",
        digits: "请只输入数字。",
        creditcard: "请输入一个有效的信用卡号。",
        equalTo: "请再次输入相同的值。",
        maxlength: jQuery.validator.format("请输入不超过{0}个字符。"),
        minlength: jQuery.validator.format("请输入至少{0}个字符。"),
        rangelength: jQuery.validator.format("请输入介于{0}和{1}个长的字符值。"),
        range: jQuery.validator.format("请输入介于{0}和{1}的值。"),
        max: jQuery.validator.format("请输入一个小于或等于{0}的值。"),
        min: jQuery.validator.format("请输入一个大于或等于{0}的值。")
    });
    //$.validator.setDefaults({
    //    submitHandler: function () {
            
    //    }
    //});

    $().ready(function() {
        // validate the comment form when it is submitted
        $("#commentForm").validate();

        // validate signup form on keyup and submit
        $("#signupForm").validate({
            rules: {
                firstname: "required",
                lastname: "required",
                username: {
                    required: true,
                    minlength: 2
                },
                password: {
                    required: true,
                    minlength: 5
                },
                confirm_password: {
                    required: true,
                    minlength: 5,
                    equalTo: "#password"
                },
                email: {
                    required: true,
                    email: true
                },
                topic: {
                    required: "#newsletter:checked",
                    minlength: 2
                },
                agree: "required"
            },
            messages: {
                firstname: "Please enter your firstname",
                lastname: "Please enter your lastname",
                username: {
                    required: "Please enter a username",
                    minlength: "Your username must consist of at least 2 characters"
                },
                password: {
                    required: "Please provide a password",
                    minlength: "Your password must be at least 5 characters long"
                },
                confirm_password: {
                    required: "Please provide a password",
                    minlength: "Your password must be at least 5 characters long",
                    equalTo: "Please enter the same password as above"
                },
                email: "Please enter a valid email address",
                agree: "Please accept our policy"
            }
        });

        // propose username by combining first- and lastname
        $("#username").focus(function() {
            var firstname = $("#firstname").val();
            var lastname = $("#lastname").val();
            if(firstname && lastname && !this.value) {
                this.value = firstname + "." + lastname;
            }
        });

        //code to hide topic selection, disable for demo
        var newsletter = $("#newsletter");
        // newsletter topics are optional, hide at first
        var inital = newsletter.is(":checked");
        var topics = $("#newsletter_topics")[inital ? "removeClass" : "addClass"]("gray");
        var topicInputs = topics.find("input").attr("disabled", !inital);
        // show when newsletter is checked
        newsletter.click(function() {
            topics[this.checked ? "removeClass" : "addClass"]("gray");
            topicInputs.attr("disabled", !this.checked);
        });
    });


}();