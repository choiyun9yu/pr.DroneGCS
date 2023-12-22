package com.gaion.kisadashboardjava.controller;

import com.gaion.kisadashboardjava.service.AiDroneService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/")
public class DroneController {

    @Autowired
    private AiDroneService aiDroneService;

    @GetMapping
    public String hello() {
        return "Hello, World!";
    }

    @GetMapping("/api/realtime")
    public List<String> realtime() {
        return aiDroneService.getDistinctDroneIds();
    }
}