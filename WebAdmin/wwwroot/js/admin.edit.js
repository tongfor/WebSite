$(document).ready(function () {
    $("input").change(function (e) {
        var obj = $(e.target);
        obj.val(obj.val().trim());
    });

    $("#cancel").click(function () {
        window.top.tb_remove();
    });

    //var mainForm = $("#submit").parent().parent();
    //mainForm.submit(function () {
    //    if (mainForm.valid()) {
    //        $("#submitloading").show();
    //        marinFromSubmit();
    //    } else {
    //        $(".validation-summary-errors").hide(8000);
    //    }
    //});    

    $("#mainForm").validate({
        submitHandler: marinFromSubmit
    });

    function marinFromSubmit() {
        var aa = 111;
        //$("#mainForm").submit(function () {
            $.ajax({
                url: $("#mainForm").attr("action"),
                data: $("#mainForm").serialize(),
                type: "POST",
                beforeSend: function () {
                    $("#submit").attr("disabled", "true");//防止连击
                    $("#submitloading").show();
                },
                success: function (result, status) {
                    if (result && "success" === status && 0 === result.status) {
                        alert(result.msg);
                        window.top.location.reload();
                        window.top.tb_remove();
                    }
                    $("#submitloading").hide();
                    $("#submit").removeAttr("disabled");
                },
                error: function (xhr, status, error) {
                    $("#submitloading").hide();
                    $("#submit").attr("disabled", "false");
                    document.write(xhr.responseText);
                    console.log(xhr);
                    console.log(status);
                    console.log(error);
                }
            });
            return false;
        //});
    }   

    //$("input[type='text']").blur(function () { $(this).removeClass("highlight"); }).focus(function () { $(this).addClass("highlight"); });

    $(".integer").numeric(false, function () { alert("只能输入数字"); this.value = ""; this.focus(); });
});
