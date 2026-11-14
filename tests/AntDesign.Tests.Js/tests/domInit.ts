//Uses JSDOM to simulate dom in tests.
//Should be imported into every test that uses dom functions.
import { JSDOM } from 'jsdom';

declare global {
  namespace TestJS {
    interface Global {
      document: Document;
      window: Window;
      navigator: Navigator;
    }
  }
}

const { window } = new JSDOM('<!doctype html><html><body></body></html>');
global.document = window.document;
global.window = global.document.defaultView;
global.Node = window.Node;
global.HTMLElement = window.HTMLElement;
global.getComputedStyle = window.getComputedStyle;

export class Guid {
  static newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  static randomId(prefix: string = 'id') {
    return prefix + this.newGuid();
  }
}

export function addElementToBody(elementAsHtmlString: string, location: InsertPosition = "afterbegin", id?: string): HTMLElement {
  const bodyElement = global.window.document.getElementsByTagName("body")[0];
  if (!id) {
    if (elementAsHtmlString.includes("id=")) {
      const start = elementAsHtmlString.indexOf("id=") + 3;
      const end = elementAsHtmlString.indexOf('"', start + 1) - 1;
      id = elementAsHtmlString.substr(start + 1, end - start);
    }
    else {
      throw 'When creating an element for tests, id is mandatory.';
    }
  }  
  bodyElement.insertAdjacentHTML(location, elementAsHtmlString);
  return global.window.document.getElementById(id);
}