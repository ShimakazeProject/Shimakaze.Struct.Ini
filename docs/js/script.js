// 去他的 JavaScript, 那是给人看的东西嘛!?
// Cookie管理
var Cookie = /** @class */ (function () {
    function Cookie() {
    }
    // 获取cookie
    Cookie.get = function (key) {
        var key = key + "=";
        var cookieArray = document.cookie.split(';');
        for (var i = 0; i < cookieArray.length; i++) {
            var cookie = cookieArray[i].trim();
            if (cookie.indexOf(key) == 0)
                return cookie.substring(key.length, cookie.length);
        }
        return "";
    };
    // 写入cookie
    Cookie.set = function (key, value, exdays) {
        if (exdays === void 0) { exdays = 30; }
        var date = new Date();
        date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + date.toUTCString();
        document.cookie = key + "=" + value + "; " + expires;
    };
    return Cookie;
}());
// 主题管理
var Theme = /** @class */ (function () {
    function Theme() {
    }
    // 从cookie中读取主题
    Theme.loadThemeFromCookie = function () {
        var theme = Cookie.get("theme");
        if (theme != "")
            Theme.change(theme);
        else if (theme != "" && theme != null) {
            Cookie.set("theme", "themeable-dark");
            Theme.change("themeable-dark");
        }
    };
    // 修改主题
    Theme.change = function (theme) {
        var links = document.getElementsByTagName("link");
        for (var i = 0; i < links.length; i++) {
            var link = links[i];
            if (link.className == "theme") {
                if (link.title == theme)
                    link.disabled = false;
                else
                    link.disabled = true;
            }
        }
        Theme.theme = theme;
    };
    return Theme;
}());
Theme.loadThemeFromCookie();
