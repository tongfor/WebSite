$(document).ready(function () {
    $("#cancel").click(function () {
        window.top.tb_remove();
    });

    //var mainForm = $("#submit").parent().parent();
    //mainForm.submit(function () {
    //    if (mainForm.valid()) {
    //        $("#submitloading").show();
    //    } else {
    //        $(".validation-summary-errors").hide(8000);
    //    }
    //});

    $("#mainForm").submit(function () {
        $.ajax({
            url: $(this).attr("action"),
            data: $(this).serialize(),
            type: "POST",
            beforeSend: function () {               
                $("#submitloading").show();
            },
            success: function (result, status) {
                if (result && "success" === status && 0 == result.status) {
                    alert(result.msg);
                    window.top.location.reload();
                    window.top.tb_remove();   
                }
                $("#submitloading").hide();
            },
            error(xhr, status, error) {
                $("#submitloading").hide();
                console.log(xhr);
                console.log(status);
                console.log(error);
            }
        });
        return false;
    });

    //$("input[type='text']").blur(function () { $(this).removeClass("highlight"); }).focus(function () { $(this).addClass("highlight"); });

    $(".integer").numeric(false, function () { alert("只能输入数字"); this.value = ""; this.focus(); });
});
