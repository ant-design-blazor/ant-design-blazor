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

export function addElementToBody(elementAsHtmlString: string, location: InsertPosition = "afterbegin"): HTMLElement {
  const bodyElement = global.window.document.getElementsByTagName("body")[0];
  bodyElement.insertAdjacentHTML(location, elementAsHtmlString);
  return global.window.document.getElementById("underTest");
}