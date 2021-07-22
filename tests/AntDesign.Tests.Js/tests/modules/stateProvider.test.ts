import { state } from '../../../../components/core/JsInterop/interop';
import { expect } from 'chai';


describe('stateProvider', () => { 
  it('objReferenceDict behaves like a dictionary', () => {
    //arrange
    let value = "testState";
    //act
    state.objReferenceDict["test"] = value
    //assert
    expect(state.objReferenceDict["test"]).to.equal(value);
  });
});