import { manipulationHelper } from "../../../../../components/core/JsInterop/modules/dom/manipulationHelper";
import { expect } from "chai";

describe("domManipulationHelper", () => {
  it("body should add a new child dom to body and then remove it from body", () => {
    //act
    const ele = document.createElement("div");
    const id = "id-r-" + 1e4 + Math.random() * 9e4;
    ele.setAttribute("id", id);
    manipulationHelper.addElementToBody(ele);
    //assert
    expect(document.body.lastChild as HTMLElement).to.equal(ele);

    manipulationHelper.delElementFromBody(ele);
    //assert
    expect(document.body.querySelector("#" + id)).to.equal(null);
  });

  it("body should add a new child dom to a container dom and then remove it from the container dom", () => {
    const container = document.createElement("div");
    const containerId = "id-container-r-" + 1e4 + Math.random() * 9e4;
    container.setAttribute("id", containerId);
    manipulationHelper.addElementToBody(container);

    const ele = document.createElement("div");
    const id = "id-r-" + 1e4 + Math.random() * 9e4;
    ele.setAttribute("id", id);
    manipulationHelper.addElementTo(ele, container);

    //assert
    expect(container.querySelector("#" + id)).to.not.equal(null);

    manipulationHelper.delElementFrom(ele, container);
    //assert
    expect(document.body.querySelector("#" + id)).to.equal(null);
  });


  it("body should add a new child dom to a container dom as prepend insert", () => {
    const container = document.createElement("div");
    const containerId = "id-container-r-" + 1e4 + Math.random() * 9e4;
    container.setAttribute("id", containerId);
    manipulationHelper.addElementToBody(container);

    const ele = document.createElement("div");
    const id = "id-r-" + 1e4 + Math.random() * 9e4;
    ele.setAttribute("id", id);
    manipulationHelper.addElementTo(ele, container, true);

    //assert
    expect(container.firstChild).to.equal(ele);
  });

  it("a attribute must be set to a dom", () => {
    const attributes = {
      class:"cls-r-" + 1e4 + Math.random() * 9e4
    }
    manipulationHelper.setDomAttribute(document.body, attributes);
    
    //assert
    expect(document.body.getAttribute("class")).to.equal(attributes.class);
  });

});
