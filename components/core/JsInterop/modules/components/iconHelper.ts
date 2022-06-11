import { generate as generateColor } from "@ant-design/colors";

export class iconHelper {
  static createIconFromfontCN(scriptUrl) {
    if (document.querySelector(`[data-namespace="${scriptUrl}"]`)) {
      return;
    }
    const script = document.createElement("script");
    script.setAttribute("src", scriptUrl);
    script.setAttribute("data-namespace", scriptUrl);
    document.body.appendChild(script);
  }

  static generateTwotoneSvgIcon(svgImg: string, primaryColor: string): string {
    svgImg = svgImg
      .replace(/['"]#333['"]/g, '"primaryColor"')
      .replace(/['"]#E6E6E6['"]/g, '"secondaryColor"')
      .replace(/['"]#D9D9D9['"]/g, '"secondaryColor"')
      .replace(/['"]#D8D8D8['"]/g, '"secondaryColor"');
    const secondaryColors = generateColor(primaryColor);
    const svg: SVGElement = iconHelper._createSVGElementFromString(svgImg);
    const children = svg.childNodes;
    const length = children.length;
    for (let i = 0; i < length; i++) {
      const child: HTMLElement = children[i] as HTMLElement;
      if (child && child.nodeName.toLowerCase() === "path") {
        if (child.getAttribute("fill") === "secondaryColor") {
          child.setAttribute("fill", secondaryColors[0]);
        } else {
          child.setAttribute("fill", primaryColor);
        }
      }
    }
    return svg.outerHTML;
  }

  private static _createSVGElementFromString(str: string): SVGElement {
    const div = document.createElement("div");
    div.innerHTML = str;
    const svg: SVGElement = div.querySelector("svg");
    return svg;
  }
}
