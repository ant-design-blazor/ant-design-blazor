export class mentionsHelper {

  private static isPopShowFlag: boolean;

  public static setPopShowFlag = function (show: boolean): void {
    mentionsHelper.isPopShowFlag = show;
  }

  public static setEditorKeyHandler = function (Mentions: any, element: HTMLTextAreaElement): void {

    const textArea = mentionsHelper.getTextarea(element);
    textArea.onkeydown = async (ev): Promise<any> => {
            //判断isPopShow不能用异步方法
      if (!mentionsHelper.isPopShowFlag) return;
      if (ev.key == "ArrowUp") {
        ev.preventDefault();
        await Mentions.invokeMethodAsync("PrevOption");
      } else if (ev.key == "ArrowDown") {
        ev.preventDefault();
        await Mentions.invokeMethodAsync("NextOption");
      }
      else if (ev.key == "Enter") {
        ev.preventDefault();
        await Mentions.invokeMethodAsync("EnterOption");
      }
            //其他按键在c#中处理
    }
  }

  public static getProp = function (e: HTMLElement, propName: string): any {
    const textArea = mentionsHelper.getTextarea(e);

    return textArea[propName];
  }

  public static getCursorXY = function (element: HTMLElement) {
    const textArea = mentionsHelper.getTextarea(element);
    const format = function (value) {
      value = value.replace(/<|>|`|"|&/g, '?');
      return value;
    };
    const inputorValue = textArea.value;
    const pos = textArea.selectionStart;
    let start_range = inputorValue.slice(0, pos);
    if (start_range.length > 0) start_range = start_range.substring(0, start_range.length - 1);
    const end_range = inputorValue.slice(pos);
    let html = format(start_range);
    html += "<span>@</span>";
    html += format(end_range);

    const div_mirror = document.createElement("div");
    div_mirror.className = "ant-mentions-measure"
    div_mirror.innerHTML = html;
    textArea.parentNode.append(div_mirror);

    const flag: HTMLSpanElement = div_mirror.querySelector("span");
        //  let flagPos = flag.getBoundingClientRect();
        //  let textAreaPos = textArea.getBoundingClientRect();
        //  let bodyPos = document.body.getBoundingClientRect();
    const left = flag.offsetLeft - textArea.scrollLeft + 16;
    const top = flag.offsetTop - textArea.scrollTop + 16;

    div_mirror.remove();
    return [left, top];
  };

  private static getTextarea(element: HTMLElement) {
    const textAreaTag = "TEXTAREA";
    let textarea = element;
    if (element.tagName != textAreaTag) {
      const allTextareas = element.getElementsByTagName(textAreaTag);
      if (allTextareas.length == 0) {
        throw "Mentions requires a textarea to be rendered, but none were found.";
      }
      textarea = allTextareas[0] as HTMLTextAreaElement;
    }

    return textarea as HTMLTextAreaElement;
  }
}
