
function listSearchExamplesScript() {

    var value = $("#SearchFieldId").val();

    $.ajax({
        type: 'GET',
        url: '/Anime/AjaxSearchList',
        data: { searchString: value }
    })
        .done(function (result) {
            $("#SuggestOutput").html(result);
            $("#AnimeSummaryId").remove();
        })

        .fail(function (xhr, status, error) {
            $("#SuggestOutput").text("No matches where found.");
        });
}
