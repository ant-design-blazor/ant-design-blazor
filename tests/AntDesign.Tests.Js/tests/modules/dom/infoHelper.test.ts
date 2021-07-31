import { infoHelper } from '../../../../../components/core/JsInterop/modules/dom/infoHelper';
import { expect } from 'chai';
import * as domInit from '../../domInit'


describe('domInfoHelper', () => { 
  it('getWindow should return proper object', () => {
    //act
    let info = infoHelper.getWindow();
    //assert
    expect(info).to.haveOwnProperty("innerWidth");
    expect(info).to.haveOwnProperty("innerHeight");
  });

  it('getElementAbsoulutePos should return domTypes.position', () => {
    //arrange
    const testedElement = domInit.addElementToBody('<div id="underTest">Test element</div>');
    //act
    let pos = infoHelper.getElementAbsolutePos(testedElement);
    //assert
    expect(pos).to.haveOwnProperty("x");
    expect(pos).to.haveOwnProperty("y");
  });
});
