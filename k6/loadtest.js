import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    high_rps_test: {
      executor: 'constant-arrival-rate',
      rate: 20000,        // requests per second
      timeUnit: '1s',
      duration: '30s',
      preAllocatedVUs: 500,
      maxVUs: 2000,
    },
  },
};

export default function () {
  const res = http.get('http://localhost:5000/price');
  check(res, {
    'status is 200': (r) => r.status === 200,
  });
}