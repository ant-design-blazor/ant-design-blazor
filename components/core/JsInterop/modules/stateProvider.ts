import { eventCallback } from './dom/types'

//Singleton
export class State {
  private static instance: State;

    //Stores references to dot net objects (components wrapped in DotNetObjectReference)
  objReferenceDict: { [key: string]: any } = {};

    //All object references must later be disposed by JS code or by .NET code.
  disposeObj(objReferenceName) {
    delete this.objReferenceDict[objReferenceName];
  }

    //Stores callback for events based on a key. Needed when
    //Event needs to be removed - the callback can be retrieved and
    //used to remove the event in question
  eventCallbackRegistry: { [key: string]: eventCallback } = {};

  oldBodyCacheStack = [];

  private constructor() { }

  static getInstance() {
    if (!this.instance) {
      this.instance = new State();
    }
    return this.instance;
  }
}

export const state = State.getInstance();