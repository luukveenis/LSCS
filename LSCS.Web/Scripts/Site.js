$(function () {

    /* Binds to any delete checklist link being clicked on the index page.
     * When the link is clicked, we eagerly hide the row from the table to
     * give the perception of instant deletion. React will then automatically
     * update the DOM when it polls and actually remove the element. */
    $("body").on("click", ".delete-checklist-link", function (e) {
        e.preventDefault();
        var link = $(this);
        var url = link.prop('href');

        $.ajax({
            method: "DELETE",
            url: url,
            success: function (msg) {
                link.parent().parent().hide();
            }
        })
        .fail(function (jqXHR, status, error) {
            alert("Failed to delete checklist: " + status);
        })
    });
});