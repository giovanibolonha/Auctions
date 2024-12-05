import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 100 },
		{ duration: '20s', target: 500 },
		{ duration: '30s', target: 1000 },
        { duration: '10s', target: 0 }
    ],
	thresholds: {
		http_req_duration: ['p(95)<500'],
		http_req_duration: ['p(99)<2000'],
		http_req_failed: ['rate<0.01'],
	},
};

export function setup() {
    const auctionUrl = 'https://localhost:7095/api/auctions';
    const auctionPayload = JSON.stringify({
		title: "Car" + Math.random().toString(),
        startTime: new Date().toISOString(),
        endTime: new Date(Date.now() + 3600 * 1000).toISOString(), // +1 hora
        startingPrice: 5000
    });

    const params = { headers: { 'Content-Type': 'application/json' } };
    const auctionResponse = http.post(auctionUrl, auctionPayload, params);

    check(auctionResponse, {
        'Auction created successfully': (res) => res.status === 201,
    });

    if (auctionResponse.status !== 201) {
        console.error('Failed to create auction:', auctionResponse.body);
        return null;
    }

    const auction = JSON.parse(auctionResponse.body);
    return auction;
}

export default function (auction) {
    if (!auction) {
        console.error('Invalid auction provided to the test.');
        return;
    }

    const bidUrl = 'https://localhost:7095/api/bids';
    const bidPayload = JSON.stringify({
        amount: Math.floor(Math.random() * 10000) + 5000,
        timestamp: new Date().toISOString(),
        auctionId: auction.id
    });

    const params = { headers: { 'Content-Type': 'application/json' } };
    const bidResponse = http.post(bidUrl, bidPayload, params);

    check(bidResponse, {
        'Bid placed successfully': (res) => res.status === 200 || res.status === 201 || res.status === 202,
		'Response time is within ideal limit': (r) => r.timings.duration < 1000, // Ideal limit of 500ms
		'Response time is within maximum acceptable limit': (r) => r.timings.duration < 2000, // Maximum limit of 2 seconds
    });

    sleep(1);
}
