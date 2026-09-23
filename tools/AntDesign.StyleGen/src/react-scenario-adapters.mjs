export function resolveReactScenario(component, key) {
  if (component === 'button' && key === 'Button renders correctly 1') {
    return {
      id: 'button-renders-correctly',
      parameters: {},
      childContent: 'Follow',
      status: 'scenario-adapted',
      source: 'components/button/__tests__/index.test.tsx:it("renders correctly")',
    };
  }

  if (component === 'button' && key === 'Button renders Chinese characters correctly 1') {
    return {
      id: 'button-chinese-characters',
      parameters: {},
      childContent: '按钮',
      status: 'scenario-adapted',
      source: 'components/button/__tests__/index.test.tsx:it("renders Chinese characters correctly")',
    };
  }

  if (component === 'alert' && key === 'renders components/alert/demo/basic.tsx extend context correctly 1') {
    return {
      id: 'alert-demo-basic',
      parameters: { Message: 'Success Text', Type: 'AlertType.Success' },
      childContent: null,
      status: 'scenario-adapted',
      source: 'components/alert/demo/basic.tsx',
    };
  }

  if (component === 'divider' && key === 'renders components/divider/demo/horizontal.tsx extend context correctly 1') {
    return {
      id: 'divider-demo-horizontal',
      parameters: {},
      childContent: null,
      status: 'scenario-adapted',
      source: 'components/divider/demo/horizontal.tsx',
    };
  }

  return {
    id: null,
    parameters: {},
    childContent: null,
    status: 'requires-scenario-adapter',
    source: null,
  };
}
