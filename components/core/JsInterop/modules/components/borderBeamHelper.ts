type BorderBeamInfo = {
  borderWidth: [number, number, number, number];
  borderRadius: string;
};

export class borderBeamHelper {
  static mount(wrapper: HTMLElement): BorderBeamInfo {
    const host = wrapper?.firstElementChild as HTMLElement | null;
    if (!host) {
      return { borderWidth: [0, 0, 0, 0], borderRadius: '0px' };
    }

    const computed = window.getComputedStyle(host);
    const parse = (value: string) => {
      const number = Number.parseFloat(value);
      return Number.isFinite(number) ? number : 0;
    };

    Array.from(wrapper.querySelectorAll(':scope > .ant-border-beam')).forEach((beam) => {
      if (beam.parentElement !== host) {
        host.appendChild(beam);
      }
    });

    return {
      borderWidth: [
        parse(computed.borderTopWidth),
        parse(computed.borderRightWidth),
        parse(computed.borderBottomWidth),
        parse(computed.borderLeftWidth),
      ],
      borderRadius: computed.borderRadius || '0px',
    };
  }
}
