//Singleton 
export class State {
    constructor() {
        //Stores references to dot net objects (components wrapped in DotNetObjectReference)
        this.objReferenceDict = {};
        //Stores callback for events based on a key. Needed when
        //Event needs to be removed - the callback can be retrieved and
        //used to remove the event in question
        this.eventCallbackRegistry = {};
        this.oldBodyCacheStack = [];
    }
    //All object references must later be disposed by JS code or by .NET code.
    disposeObj(objReferenceName) {
        delete this.objReferenceDict[objReferenceName];
    }
    static getInstance() {
        if (!this.instance) {
            this.instance = new State();
        }
        return this.instance;
    }
}
export const state = State.getInstance();
