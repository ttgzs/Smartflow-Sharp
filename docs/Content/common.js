function openWin(url, width, height) {
    var iTop = (window.screen.availHeight - 30 - height) / 2;
    var iLeft = (window.screen.availWidth - 10 - width) / 2;
    window.open(url, 'newwindow', "height=" + height + ", width=" + width + ", top=" + iTop + " left=" + iLeft + ", toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no");
}

