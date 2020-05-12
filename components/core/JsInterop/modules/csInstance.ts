const instanceObj = {

}

/**
 * Registering C# class instance references to the JS side
 * @param instance
 * @param instanceName
 */
export const initCsInstance = (instance: any, instanceName: string) => {
    instanceObj[instanceName] = instance;
}

/**
 * delete C# class instance references to the JS side
 * @param instanceName
 */
export const delCsInstance = (instanceName: string) => {
    delete instanceObj[instanceName];
}

/**
 * get C# class instance references to the JS side
 * @param instanceName
 */
export const getCsInstance = (instanceName: string) => {
    return instanceObj[instanceName];
}