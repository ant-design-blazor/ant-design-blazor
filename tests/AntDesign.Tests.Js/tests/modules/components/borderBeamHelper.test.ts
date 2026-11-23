import { expect } from 'chai';
import '../../domInit';
import { borderBeamHelper } from '../../../../../components/core/JsInterop/modules/components/borderBeamHelper';

describe('BorderBeam host geometry', () => {
  it('preserves the Size path and explicit outset while syncing border inset', () => {
    const host = document.createElement('div');
    host.style.borderRadius = '20px 20px 0 0';
    host.style.borderWidth = '1px 2px 3px 4px';
    const beam = document.createElement('div');
    beam.className = 'ant-border-beam';
    beam.style.setProperty('--ant-border-beam-size', '56px');
    host.appendChild(beam);

    borderBeamHelper.update(beam);

    expect(beam.style.getPropertyValue('--ant-border-beam-size')).to.equal('56px');
    expect(beam.style.getPropertyValue('--ant-border-beam-border-radius')).to.equal('');
    expect(beam.style.getPropertyValue('--ant-border-beam-inset-offset')).to.equal('-1px -2px -3px -4px');

    beam.style.setProperty('--ant-border-beam-inset-offset', 'calc(-1 * 0px)');
    borderBeamHelper.update(beam);

    expect(beam.style.getPropertyValue('--ant-border-beam-inset-offset')).to.equal('calc(-1 * 0px)');
  });
});
