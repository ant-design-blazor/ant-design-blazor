import * as overlayService from '../../../../../components/core/JsInterop/modules/components/overlayService';
import { expect } from 'chai';
import * as sinon from 'sinon';
import '../../domInit';

describe('OverlayService tests', () => {
    let sandbox: sinon.SinonSandbox;

    beforeEach(() => {
        sandbox = sinon.createSandbox();
        // Clean up any existing elements
        document.body.innerHTML = '';
    });

    afterEach(() => {
        sandbox.restore();
        // Clean up DOM
        document.body.innerHTML = '';
    });

    describe('positionOverlay', () => {
        it('should position overlay element at specified coordinates', () => {
            // Create a test element
            const element = document.createElement('div');
            element.id = 'test-overlay';
            document.body.appendChild(element);

            // Position the overlay
            overlayService.positionOverlay('test-overlay', 100, 200, 'body');

            // Verify positioning
            expect(element.style.position).to.equal('fixed');
            expect(element.style.left).to.equal('100px');
            expect(element.style.top).to.equal('200px');
            expect(element.style.display).to.equal('block');
            expect(parseInt(element.style.zIndex)).to.be.greaterThan(1000);
        });

        it('should warn if element not found', () => {
            const consoleWarn = sandbox.stub(console, 'warn');
            
            overlayService.positionOverlay('non-existent', 100, 200);
            
            expect(consoleWarn.calledOnce).to.be.true;
            expect(consoleWarn.firstCall.args[0]).to.include('element with id "non-existent" not found');
        });

        it('should use body as default container', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            const container = document.createElement('div');
            container.id = 'custom-container';
            document.body.appendChild(container);
            container.appendChild(element);

            overlayService.positionOverlay('test-overlay', 50, 50);

            // Should move to body
            expect(element.parentElement).to.equal(document.body);
        });

        it('should use custom container when specified', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            const container = document.createElement('div');
            container.id = 'custom-container';
            document.body.appendChild(container);
            document.body.appendChild(element);

            overlayService.positionOverlay('test-overlay', 50, 50, '#custom-container');

            expect(element.parentElement).to.equal(container);
        });

        it('should warn and use body if custom container not found', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            document.body.appendChild(element);
            const consoleWarn = sandbox.stub(console, 'warn');

            overlayService.positionOverlay('test-overlay', 50, 50, '#non-existent-container');

            expect(element.parentElement).to.equal(document.body);
            expect(consoleWarn.calledOnce).to.be.true;
            expect(consoleWarn.firstCall.args[0]).to.include('container "#non-existent-container" not found');
        });

        it('should adjust position to stay within viewport - right overflow', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            element.style.width = '200px';
            element.style.height = '100px';
            document.body.appendChild(element);

            // Mock viewport dimensions
            Object.defineProperty(window, 'innerWidth', {
                writable: true,
                configurable: true,
                value: 500
            });

            // Position near right edge
            overlayService.positionOverlay('test-overlay', 450, 100);

            const left = parseFloat(element.style.left);
            const rect = element.getBoundingClientRect();
            
            // Should be adjusted to fit within viewport
            expect(rect.right).to.be.at.most(500);
        });

        it('should adjust position to stay within viewport - bottom overflow', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            element.style.width = '200px';
            element.style.height = '100px';
            document.body.appendChild(element);

            // Mock viewport dimensions
            Object.defineProperty(window, 'innerHeight', {
                writable: true,
                configurable: true,
                value: 400
            });

            // Position near bottom edge
            overlayService.positionOverlay('test-overlay', 100, 350);

            const top = parseFloat(element.style.top);
            const rect = element.getBoundingClientRect();
            
            // Should be adjusted to fit within viewport
            expect(rect.bottom).to.be.at.most(400);
        });

        it('should adjust position if negative coordinates provided', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            element.style.width = '200px';
            element.style.height = '100px';
            document.body.appendChild(element);

            overlayService.positionOverlay('test-overlay', -50, -50);

            // After adjustment, rect positions should be at 0
            const rect = element.getBoundingClientRect();
            expect(rect.left).to.be.at.least(0);
            expect(rect.top).to.be.at.least(0);
        });

        it('should handle errors gracefully', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            document.body.appendChild(element);
            
            const consoleError = sandbox.stub(console, 'error');
            
            // Force an error by making getBoundingClientRect throw
            sandbox.stub(element, 'getBoundingClientRect').throws(new Error('Test error'));

            overlayService.positionOverlay('test-overlay', 100, 100);

            expect(consoleError.calledOnce).to.be.true;
            expect(consoleError.firstCall.args[0]).to.equal('Error in positionOverlay:');
        });
    });

    describe('removeOverlay', () => {
        it('should remove overlay from DOM', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            document.body.appendChild(element);

            expect(document.getElementById('test-overlay')).to.not.be.null;

            overlayService.removeOverlay(element);

            expect(document.getElementById('test-overlay')).to.be.null;
        });

        it('should handle null element', () => {
            expect(() => overlayService.removeOverlay(null)).to.not.throw();
        });

        it('should handle element without parent', () => {
            const element = document.createElement('div');
            expect(() => overlayService.removeOverlay(element)).to.not.throw();
        });
    });

    describe('hideOverlay', () => {
        it('should hide overlay element', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            element.style.display = 'block';
            document.body.appendChild(element);

            overlayService.hideOverlay(element);

            expect(element.style.display).to.equal('none');
        });

        it('should handle null element', () => {
            expect(() => overlayService.hideOverlay(null)).to.not.throw();
        });
    });

    describe('showOverlay', () => {
        it('should show overlay element', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            element.style.display = 'none';
            document.body.appendChild(element);

            overlayService.showOverlay(element);

            expect(element.style.display).to.equal('block');
        });

        it('should handle null element', () => {
            expect(() => overlayService.showOverlay(null)).to.not.throw();
        });
    });

    describe('window namespace integration', () => {
        beforeEach(() => {
            // Ensure window.AntDesign namespace exists in test environment
            if (typeof (window as any).AntDesign === 'undefined') {
                (window as any).AntDesign = {
                    overlayService: {
                        positionOverlay: overlayService.positionOverlay,
                        removeOverlay: overlayService.removeOverlay,
                        hideOverlay: overlayService.hideOverlay,
                        showOverlay: overlayService.showOverlay
                    }
                };
            }
        });

        it('should expose functions on window.AntDesign.overlayService', () => {
            expect((window as any).AntDesign).to.exist;
            expect((window as any).AntDesign.overlayService).to.exist;
            expect((window as any).AntDesign.overlayService.positionOverlay).to.be.a('function');
            expect((window as any).AntDesign.overlayService.removeOverlay).to.be.a('function');
            expect((window as any).AntDesign.overlayService.hideOverlay).to.be.a('function');
            expect((window as any).AntDesign.overlayService.showOverlay).to.be.a('function');
        });

        it('should work when called via window namespace', () => {
            const element = document.createElement('div');
            element.id = 'test-overlay';
            document.body.appendChild(element);

            (window as any).AntDesign.overlayService.positionOverlay('test-overlay', 150, 250, 'body');

            expect(element.style.position).to.equal('fixed');
            expect(element.style.left).to.equal('150px');
            expect(element.style.top).to.equal('250px');
        });

        it('should expose overlayService under window.AntDesign.interop after importing main', () => {
            // Importing components/main should attach interop.overlayService as part of library initialization
            // Use require to execute the module in test environment
            delete require.cache[require.resolve('../../../../../components/main')];
            require('../../../../../components/main');

            expect((window as any).AntDesign).to.exist;
            expect((window as any).AntDesign.interop).to.exist;
            expect((window as any).AntDesign.interop.overlayService).to.exist;
            expect((window as any).AntDesign.interop.overlayService.positionOverlay).to.be.a('function');
            expect((window as any).AntDesign.interop.overlayService.removeOverlay).to.be.a('function');
            expect((window as any).AntDesign.interop.overlayService.hideOverlay).to.be.a('function');
            expect((window as any).AntDesign.interop.overlayService.showOverlay).to.be.a('function');
        });
    });
});
