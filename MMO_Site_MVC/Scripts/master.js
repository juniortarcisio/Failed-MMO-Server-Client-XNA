(function (window) {
    function OnReady() {
        $("#mnuGame").click(function () {
            $("#mnuSubGame").stop().slideToggle("fast");
        });
        $("#mnuCommunity").click(function () {
            $("#mnuSubCommunity").stop().slideToggle("fast");
        });
        $("#mnuSupport").click(function () {
            $("#mnuSubSupport").stop().slideToggle("fast");
        });
        $("#mnuAbout").click(function () {
            $("#mnuSubAbout").stop().slideToggle("fast");
        });
    }

    $(document).ready(OnReady);
})(window);
// Reserved Copyright 