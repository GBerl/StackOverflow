$(() => {
    UpdateLike();
    $("#btn").on('click', function () {
        var questionid = $("#qId").val();
        var text = $("#text").val();
        var answer = {
            questionid,
            text
        }
        $("#text").val('');
        $.post(`/home/answer`, answer, function (a) {
            $("#answers").append(`<span>${text}</span>
                <lable style="color:blue">email: ${a}</lable><br/>`);
        })
    });
    var id = $("#qId").val();
    $("#likebtn").on('click', function () {
        $("#likebtn").attr('disabled', true);
        $.post(`/home/updatelikes?id=${id}`, function (l) {
            UpdateLike();
        });
    });
    function UpdateLike() {
        setInterval(() => {
            $.get(`/home/getlikescount?id=${id}`, function (likes) {
                $("#likes").val(likes);
            });
        }, 500);
    }

});