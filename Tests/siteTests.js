import { expect, use } from 'chai';
import chaiHttp from 'chai-http';
import app from '../wwwroot/js/site.js';

use(chaiHttp);

describe('API Tests', () => {
    it('should return 200 on GET /api/vehicles', (done) => {
        chai.request(app)
            .get('/api/vehicles')
            .end((err, res) => {
                expect(res).to.have.status(200);
                done();
            });
    });

    describe('POST /api/vehicles/location', () => {
        it('should update vehicle location successfully', (done) => {
            const updatedVehicle = {
                VehicleId: '1',
                Latitude: -26.2340723,
                Longitude: 28.0897142,
                Timestamp: new Date().toISOString()
            };

            chai.request(app)
                .post('/api/vehicles/location')
                .send(updatedVehicle)
                .end((err, res) => {
                    expect(res).to.have.status(200); // Expecting a 200 OK response
                    expect(res.body).to.be.an('object'); // Expecting a response body
                    expect(res.body).to.have.property('message', 'Location updated successfully');
                    done();
                });
        });

        // Test for handling errors
        it('should return an error when updating location fails', (done) => {
            const invalidVehicle = {
                VehicleId: '9999', // Assuming this ID does not exist
                Latitude: -26.9999999,
                Longitude: 28.9999999,
                Timestamp: new Date().toISOString()
            };

            chai.request(app)
                .post('/api/vehicles/location')
                .send(invalidVehicle)
                .end((err, res) => {
                    expect(res).to.have.status(400); // Expecting a 400 Bad Request
                    expect(res.body).to.be.an('object'); // Expecting a response body
                    expect(res.body).to.have.property('error'); 
                    done();
                });
        });
    });
});