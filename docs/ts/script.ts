// 去他的 JavaScript, 那是给人看的东西嘛!?
// Cookie管理
abstract class Cookie {
  // 获取cookie
  static get(key: string): string {
    var key = key + "=";
    var cookieArray = document.cookie.split(';');
    for (var i = 0; i < cookieArray.length; i++) {
      var cookie = cookieArray[i].trim();
      if (cookie.indexOf(key) == 0) return cookie.substring(key.length, cookie.length);
    }
    return "";
  }
  // 写入cookie
  static set(key: string, value: string, exdays = 30): void {
    var date = new Date();
    date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + date.toUTCString();
    document.cookie = key + "=" + value + "; " + expires;
  }
}
// 主题管理
abstract class Theme {
  // 表示当前主题的字段
  static theme: string;
  // 从cookie中读取主题
  static loadThemeFromCookie(): void {
    var theme = Cookie.get("theme");
    if (theme != "") Theme.change(theme);
    else if (theme != "" && theme != null) {
      Cookie.set("theme", "themeable-dark");
      Theme.change("themeable-dark");
    }
  }
  // 修改主题
  static change(theme: string): void {
    var links = document.getElementsByTagName("link");
    for (let i = 0; i < links.length; i++) {
      const link = links[i];
      if (link.className == "theme") {
        if (link.title == theme)
          link.disabled = false;
        else link.disabled = true;
      }
    }
    Theme.theme = theme;
  }
}
Theme.loadThemeFromCookie();