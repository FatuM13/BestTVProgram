// JS for /Home/ChooseFavourites.chtml
function checkAllCheckboxes() {
    var checkboxes = $("div > div > div > label > input");
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = true;
    }
    return false;
}

var btnSelectAll = $("#btnSelectAll");
if (btnSelectAll != null) {
    //$(this).css('background-color', 'green');
    btnSelectAll.click(checkAllCheckboxes);
}
