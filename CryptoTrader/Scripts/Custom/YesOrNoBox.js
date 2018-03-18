$(function () {
    var confirm_value = false;
    $("#buttonID").click(function () {
        $('<div role="Dialog"></div>').dialog({
            open:
                function (event, ui) {
                    $(this).html("Yes or No question?");
                },
            close: function () {
                $(this).remove();
            },

            resizable: false,
            height: 250,
            width: 250,
            modal: true,
            buttons: {
                'Yes': function () {
                    $(this).dialog('close');
                    $.ajax({
                        type: "POST",
                        url: "/Admin/AdminView",
                        data: confirm_value = true,
                    });
                },
                'No': function () {
                    $(this).dialog('close');
                    $.post('Admin/AdminView');
                }
            }
        });
    });
});
